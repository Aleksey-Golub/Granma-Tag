using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMover : MoverBase
    {
        [SerializeField] private Rigidbody _rb;

        private Collider _collider;

        public override bool IsMoved => _rb.velocity.sqrMagnitude > Constants.EPSILON;

        public override void Move(Vector3 normalizedInput, float deltaTime)
        {
            Vector3 move = CurrentMovementSpeed * Stats.MoveSpeedMultiplier.Value * normalizedInput;
            _rb.velocity = new Vector3(move.x, _rb.velocity.y, move.z);
        }

        public override void SetState(bool state)
        {
            _collider.enabled = state;
            _rb.isKinematic = !state;
        }

        protected override void InitInternal() => _collider = GetComponent<Collider>();
    }
}