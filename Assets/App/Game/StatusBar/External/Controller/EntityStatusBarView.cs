using UnityEngine;

namespace App.Game.StatusBar.External.Controller
{
    public class EntityStatusBarView : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;

        public Transform Anchor => _anchor;
    }
}