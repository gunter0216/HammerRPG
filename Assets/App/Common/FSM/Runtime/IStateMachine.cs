namespace App.Common.FSM.Runtime
{
    public interface IStateMachine
    {
        void AddState(IStage stage);
        void SyncRun();
    }
}