using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public abstract class TargetFinderBase : MonoBehaviour
    {
        [field: SerializeField] public float TargetFindDistance { get; private set; } = 5f;
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        [field: SerializeField] public Transform Origin { get; private set; }

        public void Coustruct(float distance)
        {
            TargetFindDistance = distance;
        }

        public abstract IDamageable GetNearestTargetOrNull();
    }
}