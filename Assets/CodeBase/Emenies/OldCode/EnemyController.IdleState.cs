namespace Assets.CodeBase.Enemies
{
    public partial class EnemyController
    {
        private class IdleState : EnemyStateBase
        {
            public IdleState(EnemyController controller) : base(controller)
            { }

            public override void Enter()
            {
            }

            public override void Execute(float deltaTime)
            {
                Controller._viewer.PlayIdle();
                CheckNeedAndDoTransitions();
            }

            public override void Exit()
            {
            }

            protected override bool CheckNeedAndDoTransitions() => false;
        }
    }
}