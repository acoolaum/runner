using System;
using UnityEngine;
using UnityEngine.UI;

namespace Acoolaum.Game.Ui
{
    public class GameOverWindow : MonoBehaviour
    {
        public event Action OnButtonClicked;
        
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void HandleButtonClicked()
        {
            OnButtonClicked?.Invoke();
        }
    }
}