using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public class FlatConeRaycastTargetFinder : TargetFinderBase
    {
        [SerializeField] private Vector3 _originOffset = Vector3.up;
        [SerializeField, Range(0, 180)] private float _angleOfView;
        [SerializeField, Min(0)] private float _angleDelta = 2f;

        public override IDamageable GetNearestTargetOrNull()
        {
            var rays = GetRays();
            float distance = float.MaxValue;
            IDamageable nearest = null;
            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (var ray in rays)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, TargetFindDistance, LayerMask, QueryTriggerInteraction.Ignore))
                {
                    var newDamageable = hit.transform.GetComponentInParent<IDamageable>();
                    if (newDamageable != null)
                        hits.Add(hit);
                }
            }

            foreach (var h in hits)
            {
                if (h.distance < distance)
                {
                    distance = h.distance;
                    nearest = h.transform.GetComponentInParent<IDamageable>();
                }
            }

            return nearest;
        }

        private Vector3 RayOrigin() => Origin.position + Origin.TransformDirection(_originOffset);

# if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            List<Ray> rays = GetRays();

            foreach (var ray in rays)
                Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * TargetFindDistance);
        }
#endif

        private List<Ray> GetRays()
        {
            List<Ray> rays = new List<Ray>();

            // add central Ray
            rays.Add(new Ray(RayOrigin(), Origin.forward));

            if (_angleOfView == 0)
                return rays;

            float halfAngleOfView = _angleOfView / 2;
            // add 2 border Rays
            rays.Add(new Ray(RayOrigin(), Quaternion.AngleAxis(-halfAngleOfView, Origin.up) * Origin.forward));
            rays.Add(new Ray(RayOrigin(), Quaternion.AngleAxis(halfAngleOfView, Origin.up) * Origin.forward));

            // add another Rays
            int halfRaysCount = (int)(halfAngleOfView / _angleDelta);
            for (int i = 1; i < halfRaysCount + 1; i++)
            {
                rays.Add(new Ray(RayOrigin(), Quaternion.AngleAxis(-halfAngleOfView + _angleDelta * i, Origin.up) * Origin.forward));
                rays.Add(new Ray(RayOrigin(), Quaternion.AngleAxis(halfAngleOfView - _angleDelta * i, Origin.up) * Origin.forward));
            }

            return rays;
        }
    }
}