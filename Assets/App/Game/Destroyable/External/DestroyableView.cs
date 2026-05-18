using UnityEngine;

namespace App.Game.Destroyable.External
{
    public class DestroyableView : MonoBehaviour
    {
        [SerializeField] private GameObject _preset;

        public void Destroy()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            
            Object.Instantiate(_preset, transform.position, transform.rotation, transform.parent);
            if (gameObject)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}