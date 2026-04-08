using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.CharacterWindow.External
{
    public class OpenCharacterWindowSystem : IRunSystem
    {
        private readonly CharacterWindowController _characterWindowController;

        public OpenCharacterWindowSystem(CharacterWindowController characterWindowController)
        {
            _characterWindowController = characterWindowController;
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (_characterWindowController.IsOpen())
                {
                    _characterWindowController.Close();
                }
                else
                {
                    _characterWindowController.Open();   
                }
            }
        }
    }
}