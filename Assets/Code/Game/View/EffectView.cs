using Acoolaum.Core.Components;
using UnityEngine;

namespace Acoolaum.Game.View
{
    public class EffectView : MonoBehaviour
    {
        [SerializeField] SpriteSheetAnimator _animator;
        
        private Transform _cachedTransform;
        
        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void Show(Vector3 position)
        {
            _cachedTransform.position = position;
            _animator.AnimationProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(float progress)
        {
            if (progress >= 1f)
            {
                _animator.AnimationProgressChanged -= OnProgressChanged;
                Destroy(gameObject);
            }
        }
    }
}