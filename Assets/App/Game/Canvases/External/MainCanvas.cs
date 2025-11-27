using UnityEngine;

namespace App.Game.Canvases.External
{
    public class MainCanvas : MonoBehaviour, ICanvas
    {
        public Transform GetContent()
        {
            return transform;
        }
    }
}