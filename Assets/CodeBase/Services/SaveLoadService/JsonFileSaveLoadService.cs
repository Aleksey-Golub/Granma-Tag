using Assets.CodeBase.Datas;
using Assets.CodeBase.Services.ProgressService;
using System;
using System.IO;
using UnityEngine;

namespace Assets.CodeBase.Services.SaveLoadService
{
    public class JsonFileSaveLoadService : ISaveLoadService
    {
        //private const string FILEPATH_KEY = @"C:\Users\Алексей\Desktop\testSave1.json";
        private const string FILEPATH_KEY = @"save.json";

        private readonly IPersistentProgressService _progressService;

        public JsonFileSaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress()
        {
            try
            {
                File.WriteAllText(FILEPATH_KEY, _progressService.Progress.ToJson());
                Debug.Log("progress saved");
            }
            catch (Exception e)
            {
                Debug.LogError($"Save Progress Error: {e.Message}");
            }
        }

        public PlayerProgress LoadProgressOrNull()
        {
            try
            {
                string text = File.ReadAllText(FILEPATH_KEY);
                Debug.Log("progress loaded");
                return text.ToDeserialized<PlayerProgress>();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Load Progress Error: {e.Message}");
                return null;
            }
        }
    }
}
