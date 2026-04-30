using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.FSM.Runtime
{
    public interface IState
    {
        int GetStage();
        void SyncRun();
        bool IsPredicatesCompleted();
        void SetSystems(List<IInitSystem> systems, List<IPostInitSystem> postInitSystems);
    }
}