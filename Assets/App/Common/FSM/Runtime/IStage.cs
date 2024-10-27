using System.Collections.Generic;
using App.Game;

namespace App.Common.FSM.Runtime
{
    public interface IStage
    {
        string GetName();
        void SyncRun();
        bool IsPredicatesCompleted();
        void SetSystems(List<IInitSystem> systems, List<IPostInitSystem> postInitSystems);
    }
}