using App.Common.Canvases.External;
using UnityEngine;

namespace App.Game.Canvases.External
{
    internal class BaseCanvas : MonoBehaviour, ICanvas
    {
        [SerializeField] private Canvas _canvas;

        public Canvas Canvas => _canvas;
        
        public Transform GetContent()
        {
            return transform;
        }
    }
}