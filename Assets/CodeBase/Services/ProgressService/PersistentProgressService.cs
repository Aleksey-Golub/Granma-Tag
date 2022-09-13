using Assets.CodeBase.Datas;

namespace Assets.CodeBase.Services.ProgressService
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set ; }
    }
}
