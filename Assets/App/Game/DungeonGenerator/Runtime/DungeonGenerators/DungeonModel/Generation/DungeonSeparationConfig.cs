using System;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    [Serializable]
    public class DungeonSeparationConfig
    {
        [SerializeField] private int m_Speed = 1;

        public int Speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }
    }
}