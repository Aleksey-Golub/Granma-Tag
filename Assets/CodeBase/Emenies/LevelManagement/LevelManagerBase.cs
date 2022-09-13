using Assets.CodeBase.Datas;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Player;
using Assets.CodeBase.Services.Input;
using Assets.CodeBase.Services.ProgressService;
using Assets.CodeBase.UI.EnemyIcons;
using System;
using UnityEngine;

namespace Assets.CodeBase.Emenies
{
    public abstract class LevelManagerBase : MonoBehaviour, IPauseable
    {
        [SerializeField] protected PlayerController PlayerPrefab;
        [SerializeField] protected EnemiesIconsViewer IconsViewer;

        protected Transform PlayerStartPoint;
        public bool IsPaused { get; protected set; }
        public PlayerController Player { get; protected set; }
        public abstract GameMode GameMode { get; }
        public abstract int EnemiesCount { get; }

        public event Action PlayerWin;
        public event Action PlayerLose;

        public abstract void Pause();
        public abstract void UnPause();
        public abstract void Initialize(Stats enemyStats, IInputService playerInput, IPersistentProgressService progressService, IGameTimer gameTimer);
        public void ClearScene() => Destroy(Player?.gameObject);

        protected void InvokePlayerWin()
        {
            IconsViewer.HideIcons();
            PlayerWin?.Invoke();
        }

        protected void InvokePlayerLose()
        {
            IconsViewer.HideIcons();
            PlayerLose?.Invoke();
        }

        protected void OnLateUpdate()
        {
            if (IsPaused)
                return;
            
            IconsViewer.Tick(Time.deltaTime);
        }
    }
}
