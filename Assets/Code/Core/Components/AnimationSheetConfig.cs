using System.Collections.Generic;
using UnityEngine;

namespace Acoolaum.Core.Components
{
    [CreateAssetMenu(menuName = "My Assets/" + nameof(AnimationSheetConfig), fileName = nameof(AnimationSheetConfig))]
    public class AnimationSheetConfig : ScriptableObject
    {
        [SerializeField] private List<Sprite> _sprites;

        public IReadOnlyList<Sprite> Sprites => _sprites.AsReadOnly();

        public void SetSprites(List<Sprite> sprites)
        {
            _sprites = sprites;
        }
    }
}
