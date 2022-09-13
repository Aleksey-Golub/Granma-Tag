using Assets.CodeBase.Emenies;
using UnityEngine;

namespace Assets.CodeBase.Logic.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        //[SerializeField] private ParticleSystem _hitParticle;
        [SerializeField] private int _damage;

        private bool _collisionHappened;

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (_collisionHappened)
                return;

            _collisionHappened = true;

            if (collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(transform, _damage);

                //if (damageable is BullyController)
                //    Instantiate(_hitParticle, collision.GetContact(0).point, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}