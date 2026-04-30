using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.DragItem.Runtime.Model
{
    public interface IItemSlotModel
    {
        bool CanPlaceItem(IModuleItem item);
        
        IModuleItem GetItem();
        bool PlaceItem(IModuleItem item);
        Optional<IModuleItem> RemoveItem();
        void UpItem();
        void DownItem();
    }
}