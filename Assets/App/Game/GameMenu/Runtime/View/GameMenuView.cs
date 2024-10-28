using App.Game.Settings.Runtime;
using UnityEngine;

namespace App.Game.GameMenu.Runtime.View
{
    public class GameMenuView : MonoBehaviour
    {
        [SerializeField] private GameMenuPanel m_MainMenuPanel;
        [SerializeField] private SettingsPanel m_SettingsPanel;

        public GameMenuPanel MainMenuPanel => m_MainMenuPanel;
        public SettingsPanel SettingsPanel => m_SettingsPanel;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}