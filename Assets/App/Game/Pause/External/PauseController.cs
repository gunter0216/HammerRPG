using App.Game.Pause.Runtime;
using UnityEngine;

namespace App.Game.Pause.External
{
    public class PauseController : IPauseController
    {
        public void Pause()
        {
            // todo конечно тут будет не тайм скэйл xd, возможно
            Time.timeScale = 0;
        }

        public void Unpause()
        {
            Time.timeScale = 1;
        }
    }
}