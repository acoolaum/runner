using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Acoolaum.Game.Ui
{
    public class HeroBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private List<Image> _hearts;

        private int _maxHeartsCount;
        
        public void ShowName(string name)
        {
            _nameText.text = name;
        }

        public void Init(float maxHealth)
        {
            _maxHeartsCount = Mathf.RoundToInt(maxHealth);
            if (_hearts.Count < _maxHeartsCount)
            {
                var toAdd = _maxHeartsCount - _hearts.Count;
                var prefab = _hearts[0];
                var panel = prefab.transform.parent;
                for (int i = 0; i < toAdd; i++)
                {
                    var newHeart = Instantiate(prefab, panel);
                    _hearts.Add(newHeart);
                }
            }

            for (int f = _maxHeartsCount; f < _hearts.Count; f++)
            {
                _hearts[f].gameObject.SetActive(false);
            }
        }
        
        public void ShowHealth(float health)
        {
            for (int i = 0; i < _maxHeartsCount; i++)
            {
                var isHeartEnabled = i + 1 <= health;
                _hearts[i].color = isHeartEnabled
                    ? Color.white 
                    : new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }
}