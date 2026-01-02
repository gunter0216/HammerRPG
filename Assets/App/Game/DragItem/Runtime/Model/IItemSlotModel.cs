using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.DragItem.External.Model
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