﻿using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Game.Canvases.External
{
    [MonoScoped]
    public class PopupCanvas : MonoBehaviour
    {
        public Transform GetContent()
        {
            return transform;
        }
    }
}