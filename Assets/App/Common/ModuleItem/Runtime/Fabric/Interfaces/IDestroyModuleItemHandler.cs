namespace App.Common.ModuleItem.Runtime.Fabric.Interfaces
{
    public interface IDestroyModuleItemHandler : ISortIndex
    {
        void OnItemDestroyed(IModuleItem moduleItem);
    }
}