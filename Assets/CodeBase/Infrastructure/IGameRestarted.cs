namespace Assets.CodeBase.Infrastructure
{
    public interface IGame
    {
        void RestartStage();
        void GoToNextStage();
        void ExitGame();
        void GoToCurrentStage();
    }
}