using Acoolaum.Core.Components;
using UnityEngine;

namespace Acoolaum.Game.View
{
    public class HeroView : MonoBehaviour
    {
        public SpriteSheetAnimator _spriteSheetAnimator;
        private Transform _cachedTransform;

        private Vector3 _size;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void SetPosition(Vector2 position)
        {
            _cachedTransform.position = position;
        }

        public void Play(string animationName)
        {
            var animations = _spriteSheetAnimator.Animations;
            for (int i = 0; i < animations.Count ; i++)
            {
                var sheetConfig = animations[i];
                if (sheetConfig.name == animationName)
                {
                    _spriteSheetAnimator.Play(i);
                    return;
                }
            }
        }
    }
}