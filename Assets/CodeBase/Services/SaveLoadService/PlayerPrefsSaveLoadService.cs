using Assets.CodeBase.Datas;
using Assets.CodeBase.Services.ProgressService;
using UnityEngine;

namespace Assets.CodeBase.Services.SaveLoadService
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        private const string PROGRESS_KEY = "Progress";

        private readonly IPersistentProgressService _progressService;

        public PlayerPrefsSaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());

            Debug.Log("progress saved");
        }

        public PlayerProgress LoadProgressOrNull()
        {
            Debug.Log("progress loaded");
            return PlayerPrefs.GetString(PROGRESS_KEY)?
              .ToDeserialized<PlayerProgress>();
        }
    }
}
