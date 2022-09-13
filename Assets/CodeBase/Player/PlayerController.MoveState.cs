using Assets.CodeBase.Logic;
using System;
using UnityEngine;

namespace Assets.CodeBase.Player
{
    public partial class PlayerController
    {
        private class MoveState : PlayerStateBase
        {
            private bool _inAttack;
            private IDamageable _target;

            public MoveState(PlayerController player) : base(player)
            { }

            public override void Enter()
            {
                Controller.Viewer.AttackHappening += OnViewerAttackHappening;

                Controller._canBeTarget = true;
                //Controller._gun.Off();
            }

            public override void Execute(float deltaTime)
            {
                Vector3 normalizedMovementVector = new Vector3(Controller._input.RawAxis.x, 0, Controller._input.RawAxis.y).normalized;
                Controller._mover.Move(normalizedMovementVector, deltaTime);
                if (normalizedMovementVector != Vector3.zero)
                {
                    Controller.Viewer.PlayRun();
                    Controller._rotator.RotateIn(normalizedMovementVector, deltaTime);
                }
                else
                {
                    Controller.Viewer.PlayIdle();
                }

                //if (Controller.IsPlayer)
                //{
                    if (Controller._weapon.CanAttack && _inAttack == false)
                    {
                        _target = Controller._targetFinder.GetNearestTargetOrNull();
                        if (_target != null)
                        {
                            Controller.Viewer.PlayThrowSneaker();
                            _inAttack = true;
                        }
                    }
                //}
            }

            public override void Exit()
            {
                Controller._canBeTarget = false;
                //Controller._gun.On();
                Controller.Viewer.AttackHappening -= OnViewerAttackHappening;
            }

            protected override bool CheckNeedAndDoTransitions() => false;

            private void OnViewerAttackHappening()
            {
                //if (Controller.IsPlayer)
                //{
                    Controller._weapon.Attack(_target);
                    _inAttack = false;
                //}
            }
        }
    }
}