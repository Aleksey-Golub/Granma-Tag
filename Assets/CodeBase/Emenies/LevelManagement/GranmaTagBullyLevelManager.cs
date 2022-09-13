using Assets.CodeBase.Datas;
using Assets.CodeBase.Emenies.EnemyInput;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Logic;
using Assets.CodeBase.Player;
using Assets.CodeBase.Services.Input;
using Assets.CodeBase.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.CodeBase.Emenies
{
    public class GranmaTagBullyLevelManager : LevelManagerBase
    {
        [SerializeField] private List<Route> _routes;
        [FormerlySerializedAs("_enemiesOnLevel")]
        [SerializeField] private List<EnemyOnLevel> _bulliesOnLevel;

        private readonly List<EnemyOnLevel> _currentEnemiesOnLevel = new List<EnemyOnLevel>();
        private IGameTimer _gameTimer;

        public override GameMode GameMode => GameMode.GranmaTagBully;
        public override int EnemiesCount => _bulliesOnLevel.Count;

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
            _gameTimer.TimeIsOver += PlayerGranmaLose;

            Player = Instantiate(PlayerPrefab, PlayerStartPoint.position, PlayerStartPoint.rotation);
            Player.Construct(playerInput, progressService.Progress.PlayerGranmaStats);

            _currentEnemiesOnLevel.Clear();
            _currentEnemiesOnLevel.AddRange(_bulliesOnLevel);

            IconsViewer.Initialize(_bulliesOnLevel, Player);

            foreach (var enemy in _currentEnemiesOnLevel)
            {
                var bullyInput = enemy.Enemy.GetComponent<BullyInput>();
                Route route = bullyInput.UseRandomRoute ? _routes.GetRandomItem() : null;
                bullyInput.Init(route);

                enemy.Enemy.Construct(bullyInput, enemyStats);
                enemy.Enemy.transform.position = enemy.EnemyStartPoint.position;
                enemy.Enemy.transform.rotation = enemy.EnemyStartPoint.rotation;
                enemy.Enemy.Died += OnEnemyDied;
            }
        }

        private void OnEnemyDied(IDamageable damageable)
        {
            damageable.Died -= OnEnemyDied;

            var bully = damageable as BullyController;
            var enemyOnLevel = _currentEnemiesOnLevel.FirstOrDefault(x => x.Enemy == bully);

            if (enemyOnLevel != null)
                _currentEnemiesOnLevel.Remove(enemyOnLevel);

            if (_currentEnemiesOnLevel.Count == 0)
                PlayerGranmaWin();
        }

        private void PlayerGranmaWin()
        {
            _gameTimer.TimeIsOver -= PlayerGranmaLose;

            Player.TransitionTo(PlayerController.PlayerState.Win);
            InvokePlayerWin();
        }

        private void PlayerGranmaLose()
        {
            _gameTimer.TimeIsOver -= PlayerGranmaLose;

            Player.TransitionTo(PlayerController.PlayerState.Lose);
            BulliesWin();
            InvokePlayerLose();
        }

        private void BulliesWin()
        {
            foreach (var enemy in _currentEnemiesOnLevel)
                enemy.Enemy.TransitionTo(PlayerController.PlayerState.Win);
        }
    }

    [Serializable]
    public class EnemyOnLevel
    {
        [field: SerializeField] public PlayerController Enemy { get; private set; }
        [field: SerializeField] public Transform EnemyStartPoint { get; private set; }
        [field: SerializeField] public Sprite EnemySprite { get; private set; }
    }
}
