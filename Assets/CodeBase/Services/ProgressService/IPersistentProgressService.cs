using Assets.CodeBase.Datas;

namespace Assets.CodeBase.Services.ProgressService
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}
