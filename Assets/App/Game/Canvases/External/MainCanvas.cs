﻿using App.Common.Autumn.Runtime.Attributes;
using UnityEngine;

namespace App.Game.Canvases.External
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