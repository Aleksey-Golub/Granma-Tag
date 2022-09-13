using System;
using UnityEngine;

namespace Assets.CodeBase.Obstacles
{
    public class UpPoint : MonoBehaviour
    {
        [SerializeField] private bool _useSmartAlgorithm;
        [SerializeField] private Transform _simpleEndPoint;
        [field: SerializeField] public float MinAngleForInteracteWithObstacle { get; private set; } = 25f;
        [field: SerializeField] public ObstacleAnimationType AnimationType { get; private set; }
        [field: SerializeField] public float TransitionTime { get; private set; } = 1f;
        [field: SerializeField, Range(0, 1f)] public float MinInputMagnitudeForInteractWithObstacle { get; private set; } = 0.7f;
        [Header("For Split Trajectory only")]
        [SerializeField] private bool _useSplitTrajectory;
        [SerializeField] private Transform _splitPoint;
        [SerializeField, Range(0, 1f)] private float _first_secondSegmentTimeRegulator = 0.5f;

        public Vector3 GetPosition(Vector3 startPos, float t)
        {
            var endPos =
                _useSmartAlgorithm
                ? GetSmartPositionFor(_simpleEndPoint, startPos)
                : _simpleEndPoint.position;

            var splitPos =
                _useSmartAlgorithm
                ? GetSmartPositionFor(_splitPoint, startPos)
                : _splitPoint.position;

            return LerpToEndPoint(startPos, splitPos, endPos, t);
        }

        private Vector3 GetSmartPositionFor(Transform point, Vector3 startPos)
        {
            Vector3 pos = _simpleEndPoint.InverseTransformPoint(startPos);
            return point.position + point.right * pos.x;
        }

        private Vector3 LerpToEndPoint(Vector3 startPos, Vector3 splitPos, Vector3 endPos, float t)
        {
            if (_useSplitTrajectory)
            {
                if (t <= _first_secondSegmentTimeRegulator)
                {
                    var tInternal = t * (1 / _first_secondSegmentTimeRegulator);
                    return Vector3.Lerp(startPos, splitPos, tInternal);
                }
                else
                {
                    var tInternal = (t - _first_secondSegmentTimeRegulator) / (1 - _first_secondSegmentTimeRegulator);
                    return Vector3.Lerp(splitPos, endPos, tInternal);
                }
            }
            else
            {
                return Vector3.Lerp(startPos, endPos, t);
            }
        }
    }

    public enum ObstacleAnimationType
    {
        None,
        ClimbBox,
        SlideDown,
        FenceJumping,
        WallJumping,
        ClimbBigBox,
        WallJumpingLittle,
        FenceJumpingLittle,
        Bridge2Roll,
        SprintToWallClimb,
        SoftLanding,
        HardLanding,
        ExtraHardLanding,
    }
}
