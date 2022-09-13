using Assets.CodeBase.Logic.CharacterComponents;

namespace Assets.CodeBase.Enemies
{
    public partial class EnemyController
    {
        private abstract class EnemyStateBase : CharacterStateBase<EnemyController>
        {
            protected EnemyStateBase(EnemyController controller) : base(controller)
            { }
        }
    }
}