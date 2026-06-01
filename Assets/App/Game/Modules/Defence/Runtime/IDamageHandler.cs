using App.Common.ModuleItem.Runtime;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public interface IDamageHandler
    {
        IModuleItem ModuleItem { get; }
        void Handle(HitModel model);
    }
}