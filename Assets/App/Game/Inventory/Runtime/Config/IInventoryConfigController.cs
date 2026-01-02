using System.Collections.Generic;
using App.Game.Inventory.Runtime.Config.Model;

namespace App.Game.Inventory.Runtime.Config
{
    public interface IInventoryConfigController
    {
        IReadOnlyList<IInventoryGroupConfig> GetGroups();
        int GetCols();
        int GetSlotWidth();
        int GetSlotHeight();
        int GetRows();
    }
}

