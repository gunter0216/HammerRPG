using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.Cheats.External
{
    public class OpenCheatsSystem : IRunSystem
    {
        private readonly CheatsController m_CheatsController;

        public OpenCheatsSystem(CheatsController cheatsController)
        {
            m_CheatsController = cheatsController;
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (m_CheatsController.IsOpen())
                {
                    m_CheatsController.CloseWindow();
                }
                else
                {
                    m_CheatsController.OpenWindow();   
                }
            }
        }
    }
}