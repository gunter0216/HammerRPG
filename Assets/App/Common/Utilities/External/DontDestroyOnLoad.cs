using System;
using UnityEngine;

namespace App.Common.Utilities.External
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            var transforms = GetComponentsInChildren<Transform>();
            foreach (var transform1 in transforms)
            {
                DontDestroyOnLoad(transform1);
            }
        }
    }
}