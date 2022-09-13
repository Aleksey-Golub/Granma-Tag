using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class LineRaycastTargetFinder : TargetFinderBase
    {
        [SerializeField] private Vector3 _originOffset = Vector3.up;

        public override IDamageable GetNearestTargetOrNull()
        {
            if (Physics.Raycast(RayOrigin(), Origin.forward, out RaycastHit hit, TargetFindDistance, LayerMask, QueryTriggerInteraction.Ignore))
                return hit.transform.GetComponentInParent<IDamageable>();

            return null;
        }

        private Vector3 RayOrigin() => Origin.position + Origin.TransformDirection(_originOffset);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(RayOrigin(), RayOrigin() + Origin.forward * TargetFindDistance);
        }
    }
}