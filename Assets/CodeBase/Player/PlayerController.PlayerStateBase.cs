using Assets.CodeBase.Logic.CharacterComponents;

namespace Assets.CodeBase.Player
{
    public partial class PlayerController
    {
        private abstract class PlayerStateBase : CharacterStateBase<PlayerController>
        {
            protected PlayerStateBase(PlayerController controller) : base(controller)
            { }
        }
    }
}