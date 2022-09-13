using UnityEngine;

namespace Assets.CodeBase.Logic.Weapon
{
    public class ProjectileWeapon : WeaponBase
    {
        [Header(nameof(ProjectileWeapon))]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _shootStrength = 10f;
        [SerializeField] private bool _addTorque;

        protected override void AttackInternal(IDamageable target)
        {
            Projectile newProjectile = Instantiate(_projectilePrefab, ShootPoint.position, ShootPoint.rotation);
            Vector3 to = (target.Transform.position - ShootPoint.position);
            to.y = 0;
            newProjectile.Rigidbody.velocity = to.normalized * _shootStrength;
            if (_addTorque)
                newProjectile.Rigidbody.AddTorque(10, 10, 10);
        }
    }
}