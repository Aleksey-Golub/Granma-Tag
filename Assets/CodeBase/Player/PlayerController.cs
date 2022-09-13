using Assets.CodeBase.Emenies;
using Assets.CodeBase.Datas;
using Assets.CodeBase.Logic;
using Assets.CodeBase.Logic.CharacterComponents;
using Assets.CodeBase.Logic.Weapon;
using Assets.CodeBase.Obstacles;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Services.Input;
using System;
using UnityEngine;
using static Assets.CodeBase.Logic.CharacterComponents.CharacterViewer;

namespace Assets.CodeBase.Player
{
    public partial class PlayerController : CharacterControllerBase, IDamageable, IPauseable
    {
        [Header("References")]
        [SerializeField] private MoverBase _mover;
        [SerializeField] private RotatorBase _rotator;
        [SerializeField] protected CharacterViewer Viewer;
        [SerializeField] private TargetFinderBase _targetFinder;
        [SerializeField] private ProjectileWeapon _weapon;

        [Header("Settings")]
        [SerializeField] private int _maxHP = 5;
        [SerializeField] private AnimatorState _winAnimation;
        [SerializeField] private AnimatorState _loseAnimation;
        [SerializeField, Range(0.01f, 0.99f)] private float _projectileSlowDownCoefficient = 0.1f;

        private IInputService _input;
        private Stats _stats;
        private PlayerStateBase _state;
        private UpPoint _currentPoint;
        private Transform _hitSource;

        public Transform Transform => transform;
        public bool IsAlive => HP > 0;
        [field: SerializeField] public float ParkourSpeed { get; private set; } = 1f;
        [field: SerializeField] public bool IsGranma { get; set; }

        [Header("Debug")]
        [SerializeField] private bool _canBeTarget;
        [SerializeField] private bool _isInputBlocked;
        [field: SerializeField] public int HP { get; private set; }
        public bool CanBeTarget => _canBeTarget;
        public bool IsInputBlocked => _isInputBlocked;
        public bool IsPaused { get; private set; }

        public event Action<int, int> HPChanged;
        public event Action<IDamageable> Died;
        public event Action<IDamageable> Won;

        public void Update()
        {
            if (IsPaused)
                return;

            //if (IsPlayer)
                _weapon.Tick(Time.deltaTime);

            _state?.Execute(Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_state is not MoveState)
                return;

            if (IsGranma)
            {
                var enemy = other.GetComponentInParent<BullyController>();
                if (enemy && enemy.CanBeTarget && enemy.IsAlive)
                {
                    enemy.TakeDamage(transform, 100);
                    return;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var point = other.GetComponentInParent<UpPoint>();
            if (point == null)
                return;

            var angle = Vector3.Angle(point.transform.forward, transform.forward);
            if (angle > point.MinAngleForInteracteWithObstacle || _input.RawAxis.sqrMagnitude < point.MinInputMagnitudeForInteractWithObstacle * point.MinInputMagnitudeForInteractWithObstacle)
                return;

            _currentPoint = point;

            TransitionTo(PlayerState.InteractWithObstacle);
        }

        public void Construct(IInputService input, Stats stats)
        {
            _input = input;
            _stats = stats;

            //if (IsPlayer)
                _weapon.Construct(_stats);

            Restart();
        }

        public void TransitionTo(PlayerState transitionToState)
        {
            _state?.Exit();
            _state = transitionToState switch
            {
                PlayerState.Move => new MoveState(this),
                PlayerState.Win => new WinState(this),
                PlayerState.InteractWithObstacle => new InteractWithObstacle(this),
                PlayerState.Lose => new LoseState(this),

                PlayerState.None => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };

            _state.Enter();
        }

        public void Pause() => IsPaused = true;

        public void UnPause() => IsPaused = false;

        public void TakeDamage(Transform source, int damage)
        {
            _mover.SlowDownBy(_mover.CurrentMovementSpeed * _projectileSlowDownCoefficient);
            Viewer.PlayHitEffect();
            _hitSource = source;

            HP -= damage;
            HP = HP < 0 ? 0 : HP;

            HPChanged?.Invoke(HP, _maxHP);

            if (HP == 0)
                Die();
        }

        public void Restart()
        {
            //if (IsPlayer)
                _weapon.Restart();

            Viewer.Restart();
            _mover.Construct(_stats);

            HP = _maxHP;
            HPChanged?.Invoke(HP, _maxHP);
            TransitionTo(PlayerState.Move);
        }

        private void Die()
        {
            TransitionTo(PlayerState.Lose);

            Died?.Invoke(this);
        }

        private void Lose()
        {
            if (_loseAnimation == AnimatorState.LoseFallFlat && _hitSource)
                _rotator.RotateInImmediately((transform.position - _hitSource.position).normalized);
            Viewer.PlayLose(_loseAnimation);
        }

        public enum PlayerState
        {
            None,
            Move,
            Win,
            InteractWithObstacle,
            Lose,
        }
    }
}