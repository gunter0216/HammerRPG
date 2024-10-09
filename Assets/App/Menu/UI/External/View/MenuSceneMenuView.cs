using App.Menu.UI.External.View.Panels;
using App.Menu.UI.External.View.Panels.Singleplayer;
using UnityEngine;

namespace App.Menu.UI.External.View
{
    public class MenuSceneMenuView : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel m_MainMenuPanel;
        [SerializeField] private MultiplayerPanel m_MultiplayerPanel;
        [SerializeField] private SettingsPanel m_SettingsPanel;
        [SerializeField] private SingleplayerPanel m_SingleplayerPanel;
        
        public MainMenuPanel MainMenuPanel => m_MainMenuPanel;
        public MultiplayerPanel MultiplayerPanel => m_MultiplayerPanel;
        public SettingsPanel SettingsPanel => m_SettingsPanel;
        public SingleplayerPanel SingleplayerPanel => m_SingleplayerPanel;
    }
}