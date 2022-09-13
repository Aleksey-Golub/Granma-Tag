using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class OverlapSphereTargetFinder : TargetFinderBase
    {
        public override IDamageable GetNearestTargetOrNull()
        {
            Vector3 originPos = Origin.position;
            Collider[] colliders = Physics.OverlapSphere(originPos, TargetFindDistance, LayerMask);

            IDamageable nearest = null;
            float distance = float.MaxValue;
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamageable target) && target.CanBeTarget)
                {
                    float distanceToTarget = Vector3.SqrMagnitude(target.Transform.position - originPos);
                    if (distanceToTarget < distance)
                    {
                        distance = distanceToTarget;
                        nearest = target;
                    }
                }
            }

            return nearest;
        }
    }
}