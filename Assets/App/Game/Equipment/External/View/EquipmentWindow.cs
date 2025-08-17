using UnityEngine;

namespace App.Game.Equipment.External.View
{
    public class EquipmentWindow : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}
