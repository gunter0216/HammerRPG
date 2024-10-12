using App.Common.Data.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using UnityEngine;

namespace App.Common.ApplicationQuit.External
{
    [MonoScoped]
    public class ApplicationQuitController : MonoBehaviour
    {
        [Inject] private IDataManager m_DataManager;

        private void SaveProgress()
        {
            if (m_DataManager != null)
            {
                m_DataManager.SaveByExit();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
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
    }
}