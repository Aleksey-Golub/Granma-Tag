using UnityEngine;

namespace Assets.CodeBase.Logic.CharacterComponents
{
    public abstract class CharacterStateBase<T> where T : CharacterControllerBase
    {
        protected T Controller;

        protected CharacterStateBase(T controller)
        {
            Controller = controller;
        }

        public abstract void Enter();
        public abstract void Execute(float deltaTime);
        public abstract void Exit();
        protected abstract bool CheckNeedAndDoTransitions();
    }
}