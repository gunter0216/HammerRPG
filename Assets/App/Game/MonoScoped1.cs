using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Game
{
    [MonoScoped]
    public class MonoScoped1 : MonoBehaviour
    {
        public void GetValue()
        {
            Debug.LogError("AAA");
            return;
        }
    }
}