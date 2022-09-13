using Assets.CodeBase.Datas;

namespace Assets.CodeBase.Services.SaveLoadService
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgressOrNull();
    }
}
