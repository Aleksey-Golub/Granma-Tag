namespace Assets.CodeBase.Infrastructure
{
    public interface IPauseable
    {
        bool IsPaused { get; }

        void Pause();
        void UnPause();
    }
}