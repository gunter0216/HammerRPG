using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Game.Canvases
{
    [MonoScoped]
    public class MainCanvas : MonoBehaviour
    {
        public Transform GetContent()
        {
            return transform;
        }
    }
}