using UnityEngine;

namespace App.Common.Configs.External
{
    public abstract class GameConfig : ScriptableObject
    {
    }
    
    [CreateAssetMenu]
    public class WeaponConfig : GameConfig
    {
        public float Damage;
    }

    [CreateAssetMenu]
    public class EnemyConfig : GameConfig
    {
        public float Health;
    }
}