using Assets.CodeBase.Datas;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.UI.Windows;
using System.Collections;
using UnityEngine;
using System;

namespace Assets.CodeBase.UI
{
    public class UIMediator : MonoBehaviour
    {
        [SerializeField] private GameTimerView _timerView;
        [SerializeField] private LootDataView _lootDataView;
        [SerializeField] private CatchHimPanel _catchHimPanel;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private AbilityPurchaseScreen _granmaAbilityPurchaseScreen;
        [SerializeField] private AbilityPurchaseScreen _bullyAbilityPurchaseScreen;

        private IGame _game;
        private string _sceneName;
        private GameMode _gameMode;
        private int _enemiesCount;

        public void Construct(IGame game, IGameTimer gameTimer, PlayerProgress playerProgress)
        {
            _game = game;

            _timerView.Construct(gameTimer);
            _lootDataView.Construct(playerProgress.PlayerLoot);

            _winScreen.Construct();
            _winScreen.Hide();
            _winScreen.ContinueButtonClicked += OnContinueButtonClicked;

            _loseScreen.Construct();
            _loseScreen.Hide();
            _loseScreen.RetryButtonClicked += OnRetryButtonClicked;
            _loseScreen.ExitButtonClicked += OnExitButtonClicked;

            _granmaAbilityPurchaseScreen.Construct(playerProgress, playerProgress.PlayerGranmaStats);
            _granmaAbilityPurchaseScreen.Hide();
            _granmaAbilityPurchaseScreen.TapHappened += GoToCurrentStage;

            _bullyAbilityPurchaseScreen.Construct(playerProgress, playerProgress.PlayerBullyStats);
            _bullyAbilityPurchaseScreen.Hide();
            _bullyAbilityPurchaseScreen.TapHappened += GoToCurrentStage;

            _catchHimPanel.Hide();
        }

        public void Init(string currentSceneName, GameMode gameMode, int enemiesCount)
        {
            _sceneName = currentSceneName;
            _gameMode = gameMode;
            _enemiesCount = enemiesCount;
        }

        public void ShowLoseScreen(int reaward) => _loseScreen.Show(reaward, _gameMode);

        public void ShowWinScreen(int reaward) => _winScreen.Show(reaward, _gameMode);

        public void ShowAbilityPurchaseScreen()
        {
            switch (_gameMode)
            {
                case GameMode.GranmaTagBully:
                    _granmaAbilityPurchaseScreen.Show();
                    break;
                case GameMode.BullyEscapesFromGranma:
                    _bullyAbilityPurchaseScreen.Show();
                    break;
                case GameMode.None:
                default:
                    throw new NotImplementedException($"Not implemented for {_gameMode}");
            }
        }

        private void GoToCurrentStage() => StartCoroutine(GoToCurrentStageCoroutine());

        private IEnumerator GoToCurrentStageCoroutine()
        {
            _granmaAbilityPurchaseScreen.Hide();
            _bullyAbilityPurchaseScreen?.Hide();
            yield return StartCoroutine(_catchHimPanel.ShowFor(_sceneName, _gameMode, _enemiesCount));

            _catchHimPanel.Hide();
            _game.GoToCurrentStage();
        }

        private void OnExitButtonClicked() => _game.ExitGame();

        private void OnRetryButtonClicked()
        {
            _loseScreen.Hide();
            _game.RestartStage();
        }

        private void OnContinueButtonClicked()
        {
            _winScreen.Hide();
            _game.GoToNextStage();
        }
    }
}
