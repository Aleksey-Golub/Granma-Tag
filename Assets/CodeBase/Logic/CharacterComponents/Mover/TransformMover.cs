using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class TransformMover : MoverBase
    {
        public override bool IsMoved => false;

        private bool _canMove;

        public override void Move(Vector3 normalizedInput, float deltaTime)
        {
            if (_canMove)
                transform.position += CurrentMovementSpeed * Stats.MoveSpeedMultiplier.Value * deltaTime * normalizedInput;
        }

        public override void SetState(bool state) => _canMove = state;
    }
}