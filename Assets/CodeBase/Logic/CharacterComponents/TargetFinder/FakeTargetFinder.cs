namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class FakeTargetFinder : TargetFinderBase
    {
        public override IDamageable GetNearestTargetOrNull() => null;
    }
}