using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Game.DungeonCore.External.View
{
    public class ChestView : MonoBehaviour
    {
        private Action _onClick;

        // todo
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = transform.position.z;
                if (GetComponent<SpriteRenderer>().bounds.Contains(position))
                {
                    _onClick?.Invoke();
                }
            }
        }

        public void AddClickListener(Action onClick)
        {
            _onClick = onClick;
        }
    }
}