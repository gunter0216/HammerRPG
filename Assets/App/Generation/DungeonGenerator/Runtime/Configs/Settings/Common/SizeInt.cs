using System;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.Configs.Settings.Common
{
    [Serializable]
    public class SizeInt
    {
        [SerializeField]
        private int m_Width;
        
        [SerializeField]
        private int m_Height;

        public int Width => m_Width;

        public int Height => m_Height;
    }
}