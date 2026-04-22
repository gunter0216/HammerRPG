using System.Collections.Generic;
using App.Common.Canvases.External;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace App.Game.FollowIcon.External
{
    public class FollowIconController : IInitSystem, IUpdateSystem, IFollowIconController
    {
        private readonly ISpriteLoader _spriteLoader;
        private readonly ICanvasController _canvasController;

        private Image _image;
        private RectTransform _rectTransform;

        private readonly List<object> _objects = new();

        public FollowIconController(ISpriteLoader spriteLoader, ICanvasController canvasController)
        {
            _spriteLoader = spriteLoader;
            _canvasController = canvasController;
        }

        public void Init()
        {
            var canvas = _canvasController.GetHudCanvas();

            var gameObject = new GameObject("FollowIcon");
            gameObject.transform.SetParent(canvas.GetContent(), false);
            gameObject.SetActive(false);

            _rectTransform = gameObject.AddComponent<RectTransform>();
            _rectTransform.sizeDelta = new Vector2(100, 100); // размер иконки

            _image = gameObject.AddComponent<Image>();
            _image.raycastTarget = false; // чтобы не блокировал клики

            // важно: якорь по центру (или можно подстроить)
            _rectTransform.anchorMin = new Vector2(0, 0);
            _rectTransform.anchorMax = new Vector2(0, 0);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public void Show(object obj, string spriteKey)
        {
            var sprite = _spriteLoader.Load(spriteKey);
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant load sprite.");
                return;
            }

            _image.sprite = sprite.Value;
            _objects.Add(obj);

            _image.gameObject.SetActive(true);
            OnUpdate();
        }

        public void Hide(object obj)
        {
            if (!_objects.Remove(obj))
            {
                HLogger.LogError("Cant hide.");
            }

            if (_objects.Count <= 0)
            {
                _image.gameObject.SetActive(false);
            }
        }

        public void OnUpdate()
        {
            if (_objects.Count <= 0)
                return;

            _rectTransform.position = Input.mousePosition;
        }
    }
}
