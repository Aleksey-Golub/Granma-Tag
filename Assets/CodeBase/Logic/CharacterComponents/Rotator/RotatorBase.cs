using System;
using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public abstract class RotatorBase : MonoBehaviour
    {
        [SerializeField] protected float RotationSpeed = 360f;

        public abstract void RotateIn(Vector3 direction, float deltaTime);

        public abstract void RotateInImmediately(Vector3 direction);
    }
}