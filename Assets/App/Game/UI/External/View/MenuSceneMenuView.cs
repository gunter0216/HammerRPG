using App.Game.Settings.Runtime;
using App.Menu.UI.Runtime.View.Panels;
using App.Menu.UI.Runtime.View.Panels.Singleplayer;
using UnityEngine;

namespace App.Menu.UI.Runtime.View
{
    public class MenuSceneMenuView : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel m_MainMenuPanel;
        [SerializeField] private MultiplayerPanel m_MultiplayerPanel;
        [SerializeField] private SettingsPanel m_SettingsPanel;
        [SerializeField] private SingleplayerPanel m_SingleplayerPanel;
        [SerializeField] private CreateGamePanel m_CreateGamePanel;

        public CreateGamePanel CreateGamePanel => m_CreateGamePanel;
        public MainMenuPanel MainMenuPanel => m_MainMenuPanel;
        public MultiplayerPanel MultiplayerPanel => m_MultiplayerPanel;
        public SettingsPanel SettingsPanel => m_SettingsPanel;
        public SingleplayerPanel SingleplayerPanel => m_SingleplayerPanel;
    }
}