using Assets.CodeBase.Datas;
using UnityEngine;

namespace Assets.CodeBase.Logic.Weapon
{

    public abstract class WeaponBase : MonoBehaviour
    {
        [Header(nameof(WeaponBase))]
        [SerializeField] protected Transform ShootPoint;

        private Timer _rechargeTimer = new Timer();
        private Stats _stats;

        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        
        public float Recharge => _stats.SneakerRecharge.Value;
        public virtual bool CanAttack => _rechargeTimer.Value >= Recharge;

        public void Construct(Stats stats)
        {
            _stats = stats;
        }

        public void Attack(IDamageable target)
        {
            Restart();
            AttackInternal(target);
        }

        public void Restart() => _rechargeTimer.Reset();

        protected abstract void AttackInternal(IDamageable target);
        public virtual void Off() => gameObject.SetActive(false);
        public virtual void On()
        {
            gameObject.SetActive(true);
        }

        public void Tick(float deltaTime) => _rechargeTimer.Take(deltaTime);
    }

    public enum WeaponType
    {
        None,
        Sneaker,
    }
}