using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.UI.Windows
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [Header("Animated screens")]
        [SerializeField] private GameObject _granmaHitsBully;
        [SerializeField] private GameObject _bullyWin;

        private GameObject _animatedScreen;

        public event Action ContinueButtonClicked;

        public void Construct()
        {
            _continueButton.onClick.AddListener(() => ContinueButtonClicked?.Invoke());
        }

        public void Show(int reward, GameMode gameMode)
        {
            _rewardText.text = reward.ToString();
            gameObject.SetActive(true);

            _animatedScreen = gameMode switch
            {
                GameMode.GranmaTagBully => _granmaHitsBully,
                GameMode.BullyEscapesFromGranma => _bullyWin,
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
