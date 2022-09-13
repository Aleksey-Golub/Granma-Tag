using Assets.CodeBase.Datas;
using Assets.CodeBase.Emenies.EnemyInput;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Logic;
using Assets.CodeBase.Player;
using Assets.CodeBase.Services.Input;
using Assets.CodeBase.Services.ProgressService;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Emenies.LevelManagement
{
    public class BullyEscapeFromGranmaLevelManager : LevelManagerBase
    {
        [SerializeField] private List<EnemyOnLevel> _granmasOnLevel;

        private readonly List<EnemyOnLevel> _currentEnemiesOnLevel = new List<EnemyOnLevel>();
        private IGameTimer _gameTimer;

        public override GameMode GameMode => GameMode.BullyEscapesFromGranma;
        public override int EnemiesCount => _granmasOnLevel.Count;

        private void LateUpdate() => OnLateUpdate();

        public override void Pause()
        {
            IsPaused = true;

            foreach (var p in _currentEnemiesOnLevel)
                p.Enemy.Pause();
        }

        public override void UnPause()
        {
            IsPaused = false;

            foreach (var p in _currentEnemiesOnLevel)
                p.Enemy.UnPause();
        }

        public override void Initialize(Stats enemyStats, IInputService playerInput, IPersistentProgressService progressService, IGameTimer gameTimer)
        {
            _gameTimer = gameTimer;
            PlayerStartPoint = FindObjectOfType<PlayerStartPoint>().transform;
            _gameTimer.TimeIsOver += PlayerBullyWin;

            Player = Instantiate(PlayerPrefab, PlayerStartPoint.position, PlayerStartPoint.rotation);
            Player.Construct(playerInput, progressService.Progress.PlayerBullyStats);
            Player.Died += PlayerBullyLose;

            _currentEnemiesOnLevel.Clear();
            _currentEnemiesOnLevel.AddRange(_granmasOnLevel);

            IconsViewer.Initialize(_granmasOnLevel, Player);

            foreach (var enemy in _currentEnemiesOnLevel)
            {
                var granmaInput = enemy.Enemy.GetComponent<GranmaInput>();
                granmaInput.Init(Player);

                enemy.Enemy.Construct(granmaInput, enemyStats);
                enemy.Enemy.transform.position = enemy.EnemyStartPoint.position;
                enemy.Enemy.transform.rotation = enemy.EnemyStartPoint.rotation;
            }
        }

        private void PlayerBullyWin()
        {
            _gameTimer.TimeIsOver -= PlayerBullyWin;
            Player.Died -= PlayerBullyLose;

            Player.TransitionTo(PlayerController.PlayerState.Win);
            GranmasLose();
            InvokePlayerWin();
        }

        private void PlayerBullyLose(IDamageable damageable)
        {
            _gameTimer.TimeIsOver -= PlayerBullyWin;
            Player.Died -= PlayerBullyLose;

            Player.TransitionTo(PlayerController.PlayerState.Lose);
            GranmasWin();
            InvokePlayerLose();
        }

        private void GranmasWin()
        {
            foreach (var enemy in _currentEnemiesOnLevel)
                enemy.Enemy.TransitionTo(PlayerController.PlayerState.Win);
        }

        private void GranmasLose()
        {
            foreach (var enemy in _currentEnemiesOnLevel)
                enemy.Enemy.TransitionTo(PlayerController.PlayerState.Lose);
        }
    }
}
