using Assets.CodeBase.Infrastructure;
using UnityEngine;
using TMPro;
using System;

namespace Assets.CodeBase.UI
{
    public class GameTimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        [Header("Settings")]
        [SerializeField] private ViewMode _mode = ViewMode.SecondsAndMilliseconds;

        private IGameTimer _gameTimer;

        public void Construct(IGameTimer gameTimer)
        {
            _gameTimer = gameTimer;
            _gameTimer.Updated += OnGameTimerUpdated;
        }

        private void OnGameTimerUpdated(int minutes, int seconds, int milliseconds)
        {
            int viewMilliseconds = milliseconds / 10;

            switch (_mode)
            {
                case ViewMode.None:
                    _timerText.text = String.Empty;
                    break;
                case ViewMode.Minutes:
                    _timerText.text = $"{minutes.ToString("00")}";
                    break;
                case ViewMode.Seconds:
                    _timerText.text = $"{seconds.ToString("00")}";
                    break;
                case ViewMode.MinutesAndSeconds:
                    _timerText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
                    break;
                case ViewMode.SecondsAndMilliseconds:
                    _timerText.text = $"{seconds.ToString("00")}:{viewMilliseconds.ToString("00")}";
                    break;
                case ViewMode.MinutesAndSecondsAndMilliseconds:
                    _timerText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}:{viewMilliseconds.ToString("00")}";
                    break;
                default:
                    throw new NotImplementedException($"Not implemented for {_mode}");
            }
        }

        public enum ViewMode
        {
            None,
            Minutes,
            Seconds,
            MinutesAndSeconds,
            SecondsAndMilliseconds,
            MinutesAndSecondsAndMilliseconds,
        }
    }
}
