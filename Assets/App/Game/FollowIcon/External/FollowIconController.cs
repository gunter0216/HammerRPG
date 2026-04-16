using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.FollowIcon.External
{
    public class FollowIconController : IInitSystem, IUpdateSystem, IFollowIconController
    {
        private readonly ISpriteLoader _spriteLoader;
        
        private SpriteRenderer _spriteRenderer;

        private readonly List<object> _objects = new();
        private Camera _camera;

        public FollowIconController(ISpriteLoader spriteLoader)
        {
            _spriteLoader = spriteLoader;
        }

        public void Init()
        {
            var gameObject = new GameObject("FollowIcon");
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.localScale = Vector3.one * 0.5f;
            gameObject.SetActive(false);
            
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.drawMode = SpriteDrawMode.Simple;
            _spriteRenderer.size = new UnityEngine.Vector2(1, 1);
            _spriteRenderer.sortingOrder = 10;

            _camera = Camera.main;
        }


        public void Show(object obj, string spriteKey)
        {
            var sprite = _spriteLoader.Load(spriteKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError($"Cant load sprite.");
                return;
            }

            _spriteRenderer.sprite = sprite.Value;
            _objects.Add(obj);
            _spriteRenderer.gameObject.SetActive(true);
        }

        public void Hide(object obj)
        {
            if (!_objects.Remove(obj))
            {
                HLogger.LogError("Cant hide.");
            }

            if (_objects.Count <= 0)
            {
                _spriteRenderer.gameObject.SetActive(false);
            }
        }

        public void OnUpdate()
        {
            if (_objects.Count <= 0)
            {
                return;
            }

            var position =  _camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            _spriteRenderer.transform.position = position;
        }
    }
}
