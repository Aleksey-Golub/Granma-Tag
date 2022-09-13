using System.Collections.Generic;

namespace Assets.CodeBase.Infrastructure
{
    public class PauseManager : IPauseManager
    {
        private readonly List<IPauseable> _pauseables = new List<IPauseable>();

        public bool IsPaused { get; private set; }

        public void Pause()
        {
            IsPaused = true;

            foreach (var p in _pauseables)
                p.Pause();
        }

        public void UnPause()
        {
            IsPaused = false;

            foreach (var p in _pauseables)
                p.UnPause();
        }

        public void Add(IPauseable pauseable) => _pauseables.Add(pauseable);

        public void Remove(IPauseable pauseable) => _pauseables.Remove(pauseable);
    }
}
