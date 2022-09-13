using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.CodeBase.UI.Windows
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [Header("Animated screens")]
        [SerializeField] private GameObject _granmaTired;
        [SerializeField] private GameObject _granmaHitsBully;

        private GameObject _animatedScreen;

        public event Action RetryButtonClicked;
        public event Action ExitButtonClicked;

        public void Construct()
        {
            _retryButton.onClick.AddListener(() => RetryButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
        }

        public void Show(int reward, GameMode gameMode)
        {
            _rewardText.text = reward.ToString();
            gameObject.SetActive(true);

            _animatedScreen = gameMode switch
            {
                GameMode.GranmaTagBully => _granmaTired,
                GameMode.BullyEscapesFromGranma => _granmaHitsBully,
                GameMode.None => throw new NotImplementedException($"Not implemented for {gameMode}"),
                _ => throw new NotImplementedException($"Not implemented for {gameMode}"),
            };

            _animatedScreen.SetActive(true);
        }

        public void Hide()
        {
            _animatedScreen?.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
