using System;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    public class DoorView : MonoBehaviour
    {
        private Action _onClick;

        // todo
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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