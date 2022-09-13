using UnityEngine;
using Assets.CodeBase.Datas;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public abstract class MoverBase : MonoBehaviour
    {
        [field: SerializeField] public float StartMovementSpeed { get; private set; } = 3f;
        [field: SerializeField] public float CurrentMovementSpeed { get; private set; }

        protected Stats Stats;

        public void Construct(Stats stats)
        {
            CurrentMovementSpeed = StartMovementSpeed;
            Stats = stats;
            InitInternal();
        }

        public void SlowDownBy(float delta) => CurrentMovementSpeed -= delta;

        protected virtual void InitInternal() 
        { }

        public abstract bool IsMoved { get; }

        public abstract void Move(Vector3 normalizedInput, float deltaTime);

        public abstract void SetState(bool state);
    }
}