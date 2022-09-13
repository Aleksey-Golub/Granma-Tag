using System;

namespace Assets.CodeBase.Infrastructure
{
    public interface ISceneLoader
    {
        string CurrentSceneName { get; }

        void Load(string sceneName, Action onLoaded = null);
    }
}