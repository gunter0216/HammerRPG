using App.Common.Data.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.DataContainer.Runtime.Data;
using App.Common.FSM.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Update.External;
using Zenject;

namespace App.Core.Startups.External
{
    public abstract class Configurator
    {
        private DiContainer m_Container;
        private FSMRegistrar m_FsmRegistrar;
        private DataRegistrar m_DataRegistrar;

        protected DiContainer Container => m_Container;
        protected FSMRegistrar FsmRegistrar => m_FsmRegistrar;
        protected DataRegistrar DataRegistrar => m_DataRegistrar;

        public void SetDiContainer(DiContainer container)
        {
            m_Container = container;
        }
        
        public void SetFSMRegistrator(FSMRegistrar fsmRegistrar)
        {
            m_FsmRegistrar = fsmRegistrar;
        }

        public void SetDataRegistrator(DataRegistrar dataRegistrar)
        {
            m_DataRegistrar = dataRegistrar;
        }

        public abstract void Configuration();

        protected void RegisterUpdate<T>(UpdateStage stage) where T : class, IUpdateSystem
        {
            UpdateRegistrar.Register<T>(stage);
        }
        
        protected void BindSingle<T>()
        {
            Container.BindInterfacesAndSelfTo<T>().AsSingle().NonLazy();
        }

        protected void RegisterFSM<T>(FSMStage stage, StageOrders order) where T : class, IInitSystem
        {
            FsmRegistrar.Register<T>(stage, order);
        }
        
        protected void RegisterData<T>() where T : class, IData
        {
            DataRegistrar.Register<T>();
            if (typeof(IContainerData).IsAssignableFrom(typeof(T)))
            {
                DataContainerRegistrar.Register<T>();
            }
        }

        public virtual void OnResolved() { }
    }
}