using Assets.CodeBase.Services.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Emenies.EnemyInput
{
    public class BullyInput : MonoBehaviour, IInputService
    {
        [SerializeField, Min(Constants.EPSILON)] private float _stopDistance = 0.5f;
        [SerializeField] private Route _defaultRoute;

        [field: SerializeField] public bool UseRandomRoute { get; private set; }

        [Header("Debug")]
        [SerializeField] private WayPoint _currentWayPoint;
        
        private List<WayPoint> _wayPoints = new List<WayPoint>();
        private float _stopDistanceSqrMagnitude;
        private int _wayPointIndex;

        public Vector2 RawAxis => GetMoveDirection();

        public void Init(Route route = null)
        {
            _wayPoints = route == null ? _defaultRoute.WayPoints : route.WayPoints;

            _stopDistanceSqrMagnitude = _stopDistance * _stopDistance;

            _wayPointIndex = 0;
            _currentWayPoint = _wayPoints[_wayPointIndex];
        }

        private Vector2 GetMoveDirection()
        {
            Vector3 toCurrentWayPoint = _currentWayPoint.transform.position - transform.position;
            if (toCurrentWayPoint.sqrMagnitude <= _stopDistanceSqrMagnitude)
            {
                _wayPointIndex++;
                if (_wayPointIndex > _wayPoints.Count - 1)
                    _wayPointIndex = 0;

                _currentWayPoint = _wayPoints[_wayPointIndex];
            }

            float x = Mathf.Clamp(toCurrentWayPoint.x, -1, 1);
            float z = Mathf.Clamp(toCurrentWayPoint.z, -1, 1);

            return new Vector2(x, z);
        }
    }
}
