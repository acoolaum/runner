using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Acoolaum.Core.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSheetAnimator : MonoBehaviour
    {
        public event Action<float> AnimationProgressChanged; 

        public IReadOnlyList<AnimationSheetConfig> Animations => _animations;

        [SerializeField] private int _framesPerSecond = 30;
        [SerializeField] private List<AnimationSheetConfig> _animations;

        private AnimationSheetConfig _currentAnimation;
        private SpriteRenderer _spriteRenderer;
        private float _time = 0f;
        private int _lastFrameIndex;

        public void Play(int index)
        {
            if (_currentAnimation == _animations[index])
            {
                return;
            }

            _currentAnimation = _animations[index];
            _lastFrameIndex = -1;
            _time = 0f;
            ChangeFrame(0);
        }
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (_animations == null)
            {
                return;
            }
            Play(0);
            AnimationProgressChanged?.Invoke(0f);
        }
        void Update()
        {
            if (_animations == null)
            {
                return;
            }

            if (_currentAnimation == null)
            {
                Play(0);
                return;
            }

            _time += Time.deltaTime;
            var frameIndex = Mathf.FloorToInt(_time * _framesPerSecond) % _currentAnimation.Sprites.Count;
            ChangeFrame(frameIndex);
            AnimationProgressChanged?.Invoke(_time / ((float)_currentAnimation.Sprites.Count / _framesPerSecond));
        }
        void ChangeFrame(int frameIndex)
        {
            if (_lastFrameIndex == frameIndex) {
                return;
            }

            _spriteRenderer.sprite = _currentAnimation.Sprites[frameIndex];
            _lastFrameIndex = frameIndex;
        }
    }
}