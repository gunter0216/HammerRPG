using App.Common.Configs.External;
using UnityEngine;

namespace App.Game.Inventory.Runtime.Config
{
    public class InventoryGameConfig : GameConfig
    {
        [SerializeField] private int _cols;
        [SerializeField] private int _rows;

        public InventoryGameConfig()
        {
        }

        public int Cols => _cols;
        public int Rows => _rows;
    }
}