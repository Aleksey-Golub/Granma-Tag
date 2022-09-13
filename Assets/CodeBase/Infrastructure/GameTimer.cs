using System;

namespace Assets.CodeBase.Infrastructure
{
    public class GameTimer : IGameTimer, IPauseable
    {
        private float _value;
        // private int _seconds;
        private bool _running;

        public bool IsPaused { get; private set; }

        public event Action<int, int, int> Updated;
        public event Action TimeIsOver;
        public event Action<bool> StateChanged;

        public GameTimer(float startValue)
        {
            Restart(startValue);
        }

        public void Tick(float deltaTime)
        {
            if (IsPaused)
                return;

            if (_running == false)
                return;

            _value -= deltaTime;

            if (_value <= 0)
            {
                _value = 0;
                TimeIsOver?.Invoke();
                Updated?.Invoke(0, 0, 0);
                Stop();
                return;
            }

            CheckNeedUpdate();
        }

        public void Restart(float startValue)
        {
            _running = true;
            StateChanged?.Invoke(_running);

            _value = startValue;
            //_seconds = 0;

            CheckNeedUpdate();
        }

        public void Stop()
        {
            _running = false;
            StateChanged?.Invoke(_running);
        }

        public void Pause() => IsPaused = true;

        public void UnPause() => IsPaused = false;

        private void CheckNeedUpdate()
        {
            int minutes = (int)_value / 60;
            int seconds = (int)_value % 60;
            int milliseconds = (int)((_value - (int)_value) * 1000);
            //if (_seconds != seconds)
            //{
            //    _seconds = seconds;
            //    Updated?.Invoke(minutes, seconds, milliseconds);
            //}
            Updated?.Invoke(minutes, seconds, milliseconds);
        }
    }
}
