using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Game.Equipment.Runtime.Data.Model;

namespace App.Game.Equipment.Runtime.Data
{
    public interface IEquipmentDataController
    {
        IReadOnlyList<EquipmentSlotData> GetSlots();
        bool RemoveItem(string slot);
        void SetItem(string slot, DataReference dataReference);
    }
}