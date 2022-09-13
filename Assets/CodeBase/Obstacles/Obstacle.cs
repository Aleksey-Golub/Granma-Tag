using System;
using UnityEngine;

namespace Assets.CodeBase.Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Trajectory _trajectory;

        public Trajectory Trajectory => _trajectory;

        private void OnTriggerEnter(Collider other)
        {
            print(gameObject.name + "collider with" + other.gameObject.name);
        }
    }

    [Serializable]
    public class Trajectory
    {
        public Transform _firstPoint;
        public Transform _zenith;
        public Transform _secondPoint;
    }
}
