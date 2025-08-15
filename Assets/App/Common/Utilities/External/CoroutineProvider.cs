using System.Collections;
using UnityEngine;

namespace App.Common.Utilities.External
{
    public class CoroutineProvider : MonoBehaviour
    {
        private static CoroutineProvider m_Self;

        public static Coroutine DoCoroutine(IEnumerator coroutine)
        {
            if (m_Self == null)
            {
                var obj = new GameObject("CoroutineProvider");    
                m_Self = obj.AddComponent<CoroutineProvider>();
            }

            return m_Self.StartCoroutine(coroutine);
        }

        public static void Stop(IEnumerator coroutine)
        {
            if (m_Self != null)
            {
                m_Self.StopCoroutine(coroutine);
            }
            else
            {
                Debug.LogError("CoroutineProvider doesnt exist");
            }
        }

        public static void Stop(Coroutine coroutine)
        {
            if (m_Self != null)
            {
                m_Self.StopCoroutine(coroutine);
            }
            else
            {
                Debug.LogError("CoroutineProvider doesnt exist");
            }
        }

        public static void Destroy()
        {
            m_Self.StopAllCoroutines();
            Destroy(m_Self.gameObject);
            m_Self = null;
        }

        private void OnDestroy()
        {
            if (m_Self != null)
            {
                m_Self.StopAllCoroutines();
            }
        }
    }
}