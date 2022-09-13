namespace Assets.CodeBase.Player
{
    public partial class PlayerController
    {
        private class WinState : PlayerStateBase
        {
            public WinState(PlayerController controller) : base(controller)
            {}

            public override void Enter()
            {
                Controller._canBeTarget = false;
                Controller._isInputBlocked = true;
                Controller._mover.SetState(false);

                Controller.Viewer.PlayWin(Controller._winAnimation);
                //Controller.Won?.Invoke(Controller);
            }

            public override void Execute(float deltaTime)
            {

            }

            public override void Exit()
            {
                Controller._canBeTarget = true;
                Controller._isInputBlocked = false;
                Controller._mover.SetState(true);
            }

            protected override bool CheckNeedAndDoTransitions() => false;
        }
    }
}