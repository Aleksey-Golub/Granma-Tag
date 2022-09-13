using Assets.CodeBase.Logic;
using Assets.CodeBase.Logic.CharacterComponents;
using System;
using UnityEngine;

namespace Assets.CodeBase.Enemies
{
    public partial class EnemyController : CharacterControllerBase, IDamageable
    {
        [Header("References")]
        [SerializeField] private MoverBase _mover;
        [SerializeField] private RotatorBase _rotator;
        [SerializeField] private CharacterViewer _viewer;

        [Header("Settings")]
        //[SerializeField] private float _attackDistance = 1f;
        [SerializeField] private int _maxHP = 5;

        private EnemyStateBase _state;

        public Transform Transform => transform;
        public bool IsAlive => HP > 0;
        public bool CanBeTarget => true;
        public int HP { get; private set; }

        public event Action<int, int> HPChanged;
        public event Action<IDamageable> Died;

        private void Update()
        {
            _state.Execute(Time.deltaTime);
        }

        public void Construct()
        {
            HP = _maxHP;
            HPChanged?.Invoke(HP, _maxHP);

            _state = new IdleState(this);
            _state.Enter();
        }

        public void TakeDamage(Transform source, int damage)
        {
            HP -= damage;
            HP = HP < 0 ? 0 : HP;

            HPChanged?.Invoke(HP, _maxHP);

            if (HP == 0)
                Die();
        }

        private void TransitionTo(EnemyState transitionToState)
        {
            _state.Exit();
            _state = transitionToState switch
            {
                EnemyState.Idle => new IdleState(this),
                //EnemyState.MoveToTarget => new MoveToTargetState(this),
                //EnemyState.AttackTarget => new AttackTargetState(this),
                EnemyState.None => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };

            _state.Enter();
        }

        private void Die()
        {
            Died?.Invoke(this);
            //_lootSpawner.Spawn();
            Destroy(gameObject);
        }

        public enum EnemyState
        {
            None,
            Idle,
            //MoveToTarget,
            //AttackTarget
        }
    }
}