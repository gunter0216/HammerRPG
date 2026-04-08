namespace App.Game.CharacterWindow.External.Controllers
{
    public class MainInfoController
    {
        private readonly View.CharacterWindow _window;

        public MainInfoController(View.CharacterWindow window)
        {
            _window = window;
        }

        public void Init()
        {
            
        }

        public void UpdateInfo()
        {
            _window.StrengthStat.SetStatValue(1);
            _window.AgilityStat.SetStatValue(2);
            _window.IntelligenceStat.SetStatValue(3);
        }
    }
}