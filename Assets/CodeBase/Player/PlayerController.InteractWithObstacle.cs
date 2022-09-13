using Assets.CodeBase.Logic;
using UnityEngine;
using System;
using Assets.CodeBase.Obstacles;

namespace Assets.CodeBase.Player
{
    public partial class PlayerController
    {
        private class InteractWithObstacle : PlayerStateBase
        {
            private Timer _timer;
            private Vector3 _startPos;
            private Quaternion _startRot;
            private readonly float _defauldAnimatorSpeed = 1;

            public InteractWithObstacle(PlayerController controller) : base(controller)
            { }

            public override void Enter()
            {
                Controller.Viewer.SetAnimatorSpeed(Controller._stats.ParkourSpeedMultiplier.Value * Controller.ParkourSpeed);

                _timer = new Timer();
                _startPos = Controller.transform.position;
                _startRot = Controller.transform.rotation;

                Controller._canBeTarget = false;
                Controller._isInputBlocked = true;

                Controller._mover.SetState(false);

                var animationType = Controller._currentPoint.AnimationType;
                PlayAnimationFor(animationType);
            }

            public override void Execute(float deltaTime)
            {
                _timer.Take(deltaTime * Controller._stats.ParkourSpeedMultiplier.Value * Controller.ParkourSpeed);

                float t = _timer.Value / Controller._currentPoint.TransitionTime;
                t = Mathf.Clamp01(t);
                MoveToEndPoint(t);
                RotateToEndPoint(t);

                CheckNeedAndDoTransitions();
            }

            public override void Exit()
            {
                Controller.Viewer.PlayIdle();

                Controller._mover.SetState(true);

                Controller._canBeTarget = true;
                Controller._isInputBlocked = false;

                Controller.Viewer.SetAnimatorSpeed(_defauldAnimatorSpeed);
            }

            protected override bool CheckNeedAndDoTransitions()
            {
                if (_timer.Value >= Controller._currentPoint.TransitionTime)
                {
                    Controller.TransitionTo(PlayerState.Move);
                    return true;
                }
                return false;
            }

            private void RotateToEndPoint(float t)
            {
                Controller.transform.rotation = Quaternion.Slerp(_startRot, Controller._currentPoint.transform.rotation, t * 10);
            }

            private void MoveToEndPoint(float t)
            {
                Controller.transform.position = Controller._currentPoint.GetPosition(_startPos, t);
            }

            private void PlayAnimationFor(ObstacleAnimationType animationType)
            {
                switch (animationType)
                {
                    case ObstacleAnimationType.ClimbBox:
                        Controller.Viewer.PlayClimdBox();
                        break;
                    case ObstacleAnimationType.SlideDown:
                        Controller.Viewer.PlaySlideDown();
                        break;
                    case ObstacleAnimationType.FenceJumping:
                        Controller.Viewer.PlayFenceJumping();
                        break;
                    case ObstacleAnimationType.FenceJumpingLittle:
                        Controller.Viewer.PlayFenceJumpingLittle();
                        break;
                    case ObstacleAnimationType.WallJumping:
                        Controller.Viewer.PlayWallJumping();
                        break;
                    case ObstacleAnimationType.WallJumpingLittle:
                        Controller.Viewer.PlayWallJumpingLittle();
                        break;
                    case ObstacleAnimationType.ClimbBigBox:
                        Controller.Viewer.PlayClimdBigBox();
                        break;
                    case ObstacleAnimationType.Bridge2Roll:
                        Controller.Viewer.PlayBridge2Roll();
                        break;
                    case ObstacleAnimationType.SprintToWallClimb:
                        Controller.Viewer.PlaySprintToWallClimb();
                        break;
                    case ObstacleAnimationType.SoftLanding:
                        Controller.Viewer.PlaySoftLanding();
                        break;
                    case ObstacleAnimationType.HardLanding:
                        Controller.Viewer.PlayHardLanding();
                        break;
                    case ObstacleAnimationType.ExtraHardLanding:
                        Controller.Viewer.PlayExtraHardLanding();
                        break;
                    case ObstacleAnimationType.None:
                    default:
                        throw new NotImplementedException($"Not implemented for {animationType}");
                };
            }
        }
    }
}