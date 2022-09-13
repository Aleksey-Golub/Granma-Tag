using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    [RequireComponent(typeof(CharacterController))]
    public class MoverCharacterController : MoverBase
    {
        [SerializeField] private CharacterController _characterController;

        public override bool IsMoved => _characterController.velocity.sqrMagnitude > Constants.EPSILON;

        public override void Move(Vector3 normalizedInput, float deltaTime)
        {
            _characterController.Move(CurrentMovementSpeed * Stats.MoveSpeedMultiplier.Value * deltaTime * normalizedInput);
        }

        public override void SetState(bool state) => _characterController.enabled = state;
    }
}