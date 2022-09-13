using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.CodeBase.Datas;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class CharacterViewer : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEventReporter _animationEventReporter;
        [SerializeField] private ParticleSystem _landingParticle;
        [SerializeField] private List<ParticleSystem> _reactionHitParticles;
        [SerializeField] private ParticleSystem _hitParticle;

        private AnimatorState _animatorState;
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _runStateHash = Animator.StringToHash("Run");
        private readonly int _throwStateHash = Animator.StringToHash("Throw");

        private readonly int _climbBoxStateHash = Animator.StringToHash("ClimbBox");
        private readonly int _climbBigBoxStateHash = Animator.StringToHash("ClimbBigBox");
        private readonly int _slideDownStateHash = Animator.StringToHash("SlideDown");
        private readonly int _isRunBoolStateHash = Animator.StringToHash("isRun"); 
        private readonly int _fenceJumpingStateHash = Animator.StringToHash("FenceJumping");
        private readonly int _fenceJumpingLittleStateHash = Animator.StringToHash("FenceJumpingLittle");
        private readonly int _wallJumpingStateHash = Animator.StringToHash("WallJumping");
        private readonly int _wallJumpingLittleStateHash = Animator.StringToHash("WallJumpingLittle");
        private readonly int _bridge2RollStateHash = Animator.StringToHash("Bridge2Roll");
        private readonly int _sprintToWallClimbStateHash = Animator.StringToHash("SprintToWallClimb");
        private readonly int _softLandingStateHash = Animator.StringToHash("SoftLanding");
        private readonly int _hardLandingStateHash = Animator.StringToHash("HardLanding");
        private readonly int _extraHardLandingStateHash = Animator.StringToHash("ExtraHardLanding");

        private readonly int _loseFallFlatStateHash = Animator.StringToHash("LoseFallFlat");
        private readonly int _winAttackSwordStatrHash = Animator.StringToHash("WinAttackSword");
        private readonly int _winStrongGestureHash = Animator.StringToHash("WinStrongGesture");
        
        private readonly int _idleAnimClipHash = Animator.StringToHash("Idle");

        public event Action AttackStarted;
        public event Action AttackHappening;
        public event Action AttackEnded;

        private void OnEnable()
        {
            _animationEventReporter.AttackStarted += () => AttackStarted?.Invoke();
            _animationEventReporter.AttackHappening += () => AttackHappening?.Invoke();
            _animationEventReporter.AttackEnded += () => AttackEnded?.Invoke();
            _animationEventReporter.LandingHappening += () => _landingParticle.Play();
        }
        public void SetAnimatorSpeed(float value) => _animator.speed = value;

        public void PlayWin(AnimatorState _winAnimation) => PlayAnimation(_winAnimation);
        public void PlayLose(AnimatorState _loseAnimation) => PlayAnimation(_loseAnimation);
        public void PlayWallJumping() => PlayAnimation(AnimatorState.WallJumping);
        public void PlayWallJumpingLittle() => PlayAnimation(AnimatorState.WallJumpingLittle);
        public void PlayFenceJumping() => PlayAnimation(AnimatorState.FenceJumping);
        public void PlayFenceJumpingLittle() => PlayAnimation(AnimatorState.FenceJumpingLittle);
        public void PlaySlideDown() => PlayAnimation(AnimatorState.SlideDown);
        public void PlayClimdBox() => PlayAnimation(AnimatorState.ClimbBox);
        public void PlayClimdBigBox() => PlayAnimation(AnimatorState.ClimbBigBox);
        public void PlayBridge2Roll() => PlayAnimation(AnimatorState.Bridge2Roll);
        public void PlaySprintToWallClimb() => PlayAnimation(AnimatorState.SprintToWallClimb);
        public void PlaySoftLanding() => PlayAnimation(AnimatorState.SoftLanding);
        public void PlayHardLanding() => PlayAnimation(AnimatorState.HardLanding);
        public void PlayExtraHardLanding() => PlayAnimation(AnimatorState.ExtraHardLanding);
        public void PlayThrowSneaker() => _animator.SetTrigger(GetAnimHash(AnimatorState.ThrowSneaker));
        public void PlayHitEffect()
        {
            _hitParticle?.Play();
            if (_reactionHitParticles.Count > 0)
                _reactionHitParticles.GetRandomItem().Play(); 
        }

        public void PlayRun()
        {
            _animatorState = AnimatorState.Run;
            _animator.SetBool(_isRunBoolStateHash, true);
        }

        public void PlayIdle()
        {
            _animatorState = AnimatorState.Idle;
            _animator.SetBool(_isRunBoolStateHash, false);
        }

        public void Restart()
        {
            ResetAllLoseAndWinTriggers();
            _animator.Play(_idleAnimClipHash);
        }

        private void PlayAnimation(AnimatorState state)
        {
            if (_animatorState == state)
                return;

            _animator.SetBool(_isRunBoolStateHash, false);

            _animator.SetTrigger(GetAnimHash(state));
            _animatorState = state;
        }

        private int GetAnimHash(AnimatorState state)
        {
            return state switch
            {
                AnimatorState.Run => _runStateHash,
                AnimatorState.Idle => _idleStateHash,
                AnimatorState.LoseFallFlat => _loseFallFlatStateHash,
                AnimatorState.WinAttackSword => _winAttackSwordStatrHash,
                AnimatorState.WinStrongGesture => _winStrongGestureHash,
                AnimatorState.ThrowSneaker => _throwStateHash,
                AnimatorState.ClimbBox => _climbBoxStateHash,
                AnimatorState.SlideDown => _slideDownStateHash,
                AnimatorState.FenceJumping => _fenceJumpingStateHash,
                AnimatorState.FenceJumpingLittle => _fenceJumpingLittleStateHash,
                AnimatorState.WallJumping => _wallJumpingStateHash,
                AnimatorState.WallJumpingLittle => _wallJumpingLittleStateHash,
                AnimatorState.ClimbBigBox => _climbBigBoxStateHash,
                AnimatorState.Bridge2Roll => _bridge2RollStateHash,
                AnimatorState.SprintToWallClimb => _sprintToWallClimbStateHash,
                AnimatorState.SoftLanding => _softLandingStateHash,
                AnimatorState.HardLanding => _hardLandingStateHash,
                AnimatorState.ExtraHardLanding => _extraHardLandingStateHash,

                AnimatorState.None => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }

        private void ResetAllLoseAndWinTriggers()
        {
            _animator.ResetTrigger(_loseFallFlatStateHash);
            _animator.ResetTrigger(_winAttackSwordStatrHash);
            _animator.ResetTrigger(_winStrongGestureHash);
        }

        public enum AnimatorState
        {
            None,
            Run,
            Idle,
            LoseFallFlat,
            ThrowSneaker,
            WinAttackSword,
            ClimbBox,
            SlideDown,
            FenceJumping,
            WallJumping,
            ClimbBigBox,
            WallJumpingLittle,
            FenceJumpingLittle,
            WinStrongGesture,
            Bridge2Roll,
            SprintToWallClimb,
            SoftLanding,
            HardLanding,
            ExtraHardLanding,
        }
    }
}