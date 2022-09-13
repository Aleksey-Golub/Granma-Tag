using UnityEngine;
using Assets.CodeBase.Services.ProgressService;
using JetBrains.Annotations;

namespace Assets.CodeBase.UI.DEBUG
{
    internal class DebugTools : MonoBehaviour
    {
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        [UsedImplicitly()]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [UsedImplicitly()]
        public void GetMoney(int value)
        {
            _progressService.Progress.PlayerLoot.Add(value);
        }
    }
}
