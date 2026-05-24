using App.Common.ModuleItem.Runtime;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public class HitModel
    {
        private readonly IModuleItem _sourceModel;
        private readonly IModuleItem _targetModel;
        private readonly float _damage;

        public HitModel(IModuleItem sourceModel, IModuleItem targetModel, float damage)
        {
            _sourceModel = sourceModel;
            _targetModel = targetModel;
            _damage = damage;
        }

        public IModuleItem SourceModel => _sourceModel;

        public IModuleItem TargetModel => _targetModel;

        public float Damage => _damage;
    }
}