using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace App.Game.Interactions.External
{
    internal sealed class SpriteHoverHighlight : IDisposable
    {
        private const int OutlinePixels = 2;
        private const int MaxTextureSize = 512;
        private static readonly int _mainTex = Shader.PropertyToID("_MainTex");
        private static readonly int _spriteOpacity = Shader.PropertyToID("_SpriteOpacity");
        private static readonly int _maskStep = Shader.PropertyToID("_MaskStep");
        private static Material _material;

        private readonly Transform _root;
        private readonly Dictionary<Sprite, Mesh> _spriteMeshes = new();
        private readonly MaterialPropertyBlock _properties = new();
        private readonly CommandBuffer _commands = new() { name = "Interactable sprite mask" };
        private MeshRenderer _overlay;
        private Mesh _quad;
        private RenderTexture _mask;

        public SpriteHoverHighlight(Transform root)
        {
            _root = root;
        }

        public static bool IsVisible(SpriteRenderer renderer) => renderer != null && renderer.enabled &&
            !renderer.forceRenderingOff && renderer.gameObject.activeInHierarchy && renderer.sprite != null && renderer.color.a > 0f;

        public static int CompareSorting(SpriteRenderer first, SpriteRenderer second)
        {
            int layer = SortingLayer.GetLayerValueFromID(first.sortingLayerID)
                .CompareTo(SortingLayer.GetLayerValueFromID(second.sortingLayerID));
            return layer != 0 ? layer : first.sortingOrder.CompareTo(second.sortingOrder);
        }

        public void Show(SpriteRenderer[] renderers)
        {
            var localBounds = new Bounds();
            float pixelsPerUnit = 1f;
            SpriteRenderer front = null;
            foreach (var renderer in renderers)
            {
                if (!IsVisible(renderer))
                    continue;

                var matrix = GetLocalMatrix(renderer);
                var bounds = renderer.sprite.bounds;
                for (int i = 0; i < 4; i++)
                {
                    var corner = new Vector3((i & 1) == 0 ? bounds.min.x : bounds.max.x,
                        (i & 2) == 0 ? bounds.min.y : bounds.max.y);
                    var point = matrix.MultiplyPoint3x4(corner);
                    if (front == null && i == 0)
                        localBounds = new Bounds(point, Vector3.zero);
                    else
                        localBounds.Encapsulate(point);
                }

                float scale = Mathf.Min(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude);
                pixelsPerUnit = Mathf.Max(pixelsPerUnit, renderer.sprite.pixelsPerUnit / Mathf.Max(scale, .01f));
                if (front == null || CompareSorting(renderer, front) > 0)
                    front = renderer;
            }

            if (front == null)
            {
                Hide();
                return;
            }

            if (_material == null)
                _material = Resources.Load<Material>("InteractableHoverHighlight");
            if (_material == null)
                return;

            // Leave transparent texels around the object so the outline can extend past its sprite bounds.
            float padding = (OutlinePixels + 1f) / pixelsPerUnit;
            localBounds.Expand(new Vector3(2f * padding, 2f * padding));
            int width = Mathf.Clamp(Mathf.RoundToInt(localBounds.size.x * pixelsPerUnit), 1, MaxTextureSize);
            int height = Mathf.Clamp(Mathf.RoundToInt(localBounds.size.y * pixelsPerUnit), 1, MaxTextureSize);
            DrawMask(renderers, localBounds, width, height);

            if (_overlay == null)
                CreateOverlay();
            _overlay.gameObject.layer = front.gameObject.layer;
            _overlay.sortingLayerID = front.sortingLayerID;
            _overlay.sortingOrder = Mathf.Min(short.MaxValue, front.sortingOrder + 1);
            _overlay.transform.localPosition = localBounds.center;
            _overlay.transform.localScale = new Vector3(localBounds.size.x, localBounds.size.y, 1f);
            _properties.Clear();
            _properties.SetTexture(_mainTex, _mask);
            _properties.SetVector(_maskStep, new Vector4(1f / width, 1f / height));
            _overlay.SetPropertyBlock(_properties);
            _overlay.enabled = true;
        }

        public void Hide()
        {
            if (_overlay != null)
                _overlay.enabled = false;

            if (_mask != null)
            {
                RenderTexture.ReleaseTemporary(_mask);
                _mask = null;
            }
        }

        public void Dispose()
        {
            Hide();
            _commands.Release();
            foreach (var mesh in _spriteMeshes.Values)
                Object.Destroy(mesh);
            Object.Destroy(_quad);
            if (_overlay != null)
                Object.Destroy(_overlay.gameObject);
        }

        private Matrix4x4 GetLocalMatrix(SpriteRenderer renderer) => _root.worldToLocalMatrix * renderer.localToWorldMatrix *
            Matrix4x4.Scale(new Vector3(renderer.flipX ? -1 : 1, renderer.flipY ? -1 : 1, 1));

        private void DrawMask(SpriteRenderer[] renderers, Bounds bounds, int width, int height)
        {
            if (_mask == null || _mask.width != width || _mask.height != height)
            {
                Hide();
                _mask = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                _mask.filterMode = FilterMode.Point;
                _mask.wrapMode = TextureWrapMode.Clamp;
            }

            var projection = Matrix4x4.Ortho(bounds.min.x, bounds.max.x, bounds.min.y, bounds.max.y, -1f, 1f);
            _commands.Clear();
            _commands.SetRenderTarget(_mask);
            _commands.ClearRenderTarget(false, true, Color.clear);
            foreach (var renderer in renderers)
            {
                if (!IsVisible(renderer))
                    continue;

                _properties.Clear();
                _properties.SetTexture(_mainTex, renderer.sprite.texture);
                _properties.SetFloat(_spriteOpacity, renderer.color.a);
                _commands.DrawMesh(GetSpriteMesh(renderer.sprite), projection * GetLocalMatrix(renderer), _material, 0, 1, _properties);
            }
            Graphics.ExecuteCommandBuffer(_commands);
        }

        private Mesh GetSpriteMesh(Sprite sprite)
        {
            if (_spriteMeshes.TryGetValue(sprite, out var mesh))
                return mesh;

            var spriteVertices = sprite.vertices;
            var vertices = Array.ConvertAll(spriteVertices, point => (Vector3)point);
            mesh = new Mesh
            {
                name = sprite.name + " hover mask",
                vertices = vertices,
                uv = sprite.uv,
                triangles = Array.ConvertAll(sprite.triangles, index => (int)index)
            };
            _spriteMeshes.Add(sprite, mesh);
            return mesh;
        }

        private void CreateOverlay()
        {
            var overlay = new GameObject("Hover Highlight") { hideFlags = HideFlags.DontSave };
            overlay.transform.SetParent(_root, false);
            _quad = new Mesh
            {
                name = "Interactable highlight quad",
                vertices = new[] { new Vector3(-.5f, -.5f), new Vector3(-.5f, .5f), new Vector3(.5f, .5f), new Vector3(.5f, -.5f) },
                uv = new[] { Vector2.zero, Vector2.up, Vector2.one, Vector2.right },
                triangles = new[] { 0, 1, 2, 0, 2, 3 }
            };
            overlay.AddComponent<MeshFilter>().sharedMesh = _quad;
            _overlay = overlay.AddComponent<MeshRenderer>();
            _overlay.sharedMaterial = _material;
            _overlay.shadowCastingMode = ShadowCastingMode.Off;
            _overlay.receiveShadows = false;
        }
    }
}
