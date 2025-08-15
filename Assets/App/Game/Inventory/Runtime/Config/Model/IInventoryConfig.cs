using System.Collections.Generic;

namespace App.Game.Inventory.Runtime.Config
{
    public interface IInventoryConfig
    {
        IReadOnlyList<IInventoryGroupConfig> Groups { get; }
        int Cols { get; }
        int SlotWidth { get; }
        int SlotHeight { get; }
        int Rows { get; }
    }
}

