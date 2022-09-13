namespace Assets.CodeBase.Logic.Weapon
{
    public class FakeProjectileWeapon : ProjectileWeapon
    {
        public override bool CanAttack => false;
        protected override void AttackInternal(IDamageable target)
        {
        }
    }
}