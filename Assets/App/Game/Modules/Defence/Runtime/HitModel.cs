using App.Common.ModuleItem.Runtime;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public class HitModel
    {
        private readonly IModuleItem _sourceModel;
        private readonly float _damage;

        public HitModel(IModuleItem sourceModel, float damage)
        {
            _sourceModel = sourceModel;
            _damage = damage;
        }

        public IModuleItem SourceModel => _sourceModel;
        public float Damage => _damage;
    }
}