using UnityEngine;

namespace Acoolaum.Game.View
{
    public class ElementView : MonoBehaviour
    {
        private Transform _cachedTransform;
        
        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void SetPosition(Vector3 position)
        {
            _cachedTransform.position = position;
        }
    }
}