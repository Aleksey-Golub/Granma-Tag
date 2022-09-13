using System;
using UnityEngine;

namespace Assets.CodeBase.Logic
{
    public interface IDamageable
    {
        Transform Transform { get; }
        bool IsAlive { get; }
        bool CanBeTarget { get; }
        int HP { get; }
        event Action<int, int> HPChanged;
        event Action<IDamageable> Died;

        void TakeDamage(Transform source, int damage);
    }
}