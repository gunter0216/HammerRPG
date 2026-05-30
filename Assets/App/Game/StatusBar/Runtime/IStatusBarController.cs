using App.Common.ModuleItem.External;

namespace App.Game.StatusBar.Runtime
{
    public interface IStatusBarController
    {
        void Show(ModuleItemView view);
        void Hide(ModuleItemView view);
    }
}