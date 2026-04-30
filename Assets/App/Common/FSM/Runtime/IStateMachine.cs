namespace App.Common.FSM.Runtime
{
    public interface IStateMachine
    {
        void AddState(IState state);
        void SyncRun();
    }
}