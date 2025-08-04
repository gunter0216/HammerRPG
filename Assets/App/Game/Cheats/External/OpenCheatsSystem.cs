using App.Common.Autumn.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using UnityEngine;

namespace App.Game.Cheats.External
{
    [Scoped(typeof(GameSceneContext))]
    [RunSystem(0)]
    public class OpenCheatsSystem : IRunSystem
    {
        [Inject] private readonly CheatsController m_CheatsController;
        
        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.I))
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