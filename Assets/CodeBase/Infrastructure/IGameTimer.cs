using System;

namespace Assets.CodeBase.Infrastructure
{
    public interface IGameTimer
    {
        event Action<int, int, int> Updated;
        event Action TimeIsOver;
        //event Action<bool> StateChanged;
    }
}