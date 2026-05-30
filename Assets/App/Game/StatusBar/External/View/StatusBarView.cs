using UnityEngine;

namespace App.Game.StatusBar.External.View
{
    public class StatusBarView : MonoBehaviour
    {
        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }
    }
}