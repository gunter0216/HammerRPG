namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public interface IDamageHandler
    {
        string Id { get; }
        void Handle(HitModel model);
    }
}