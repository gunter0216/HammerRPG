using System;
using App.Common.Data.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UniRx;

namespace App.Common.ApplicationQuit.External
{
    public class ApplicationQuitController : IInitSystem, IDisposable
    {
        private readonly IDataManager m_DataManager;
        private readonly CompositeDisposable m_Disposables = new();

        public ApplicationQuitController(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public void Init()
        {
            Observable.EveryApplicationFocus().Subscribe(OnFocusChange).AddTo(m_Disposables);
#if UNITY_EDITOR
            Observable.OnceApplicationQuit().Subscribe(_ => OnApplicationQuit()).AddTo(m_Disposables);
#endif
        }

        private void SaveProgress()
        {
            if (m_DataManager != null)
            {
                m_DataManager.SaveProgress();
            }
        }

        private void OnFocusChange(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveProgress();
            }
        }

#if UNITY_EDITOR

        private void OnApplicationQuit()
        {
            SaveProgress();
        }
#endif
        public void Dispose()
        {
            m_Disposables?.Dispose();
        }
    }
}