using UnityEngine;

namespace App.Game.Canvases.External
{
    public class PopupCanvas : MonoBehaviour, ICanvas
    {
        public Transform GetContent()
        {
            return transform;
        }
    }
}