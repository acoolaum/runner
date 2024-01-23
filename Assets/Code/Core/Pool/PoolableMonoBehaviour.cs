using UnityEngine;

namespace Acoolaum.Core.Services
{
    public class PoolableMonoBehaviour : MonoBehaviour
    {
        public virtual void OnInit()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnRecycle()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnDispose()
        {
            Destroy(gameObject);
        }
    }
}