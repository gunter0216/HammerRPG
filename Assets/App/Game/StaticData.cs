using UnityEngine;

namespace App.Game
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        [SerializeField] public GameObject PlayerPrefab;
        [SerializeField] public float PlayerMoveSpeed;
    }
}