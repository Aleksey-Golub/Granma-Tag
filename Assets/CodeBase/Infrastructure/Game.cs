using Assets.CodeBase.CameraLogic;
using Assets.CodeBase.Datas;
using Assets.CodeBase.Emenies;
using Assets.CodeBase.Player;
using Assets.CodeBase.Services.Input;
using Assets.CodeBase.Services.ProgressService;
using Assets.CodeBase.Services.SaveLoadService;
using Assets.CodeBase.UI;
using Assets.CodeBase.UI.DEBUG;
using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class Game : MonoBehaviour, IGame, ICoroutineRunner
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private CameraParent _cameraParent;
        [SerializeField] private UIMediator _uIMediator;
        [SerializeField] private GameSettings _gameSettings;

        [Header("Debug Tools")]
        [SerializeField] private DebugTools _debugTools;
        [SerializeField] private bool _printPlayerProgress;

        private GameTimer _gameTimer;
        private IInputService _inputService;
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private ISceneLoader _sceneLoader;
        private IPauseManager _pauseManager;

        private PlayerController _player;
        private LevelData _currentLevelData;
        private LevelManagerBase _levelManager;

        private const string INITIAL = "Initial";
        private string _startSceneName = null;
        private bool DebugMode => _startSceneName != null;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            RegisterServices();
            LoadProgressOrInitNew();

            if (_sceneLoader.CurrentSceneName != INITIAL)
                _startSceneName = _sceneLoader.CurrentSceneName;

            _gameTimer = new GameTimer(10);
            _pauseManager.Add(_gameTimer);

            _uIMediator.Construct(this, _gameTimer, _progressService.Progress);
            _debugTools.Construct(_progressService);

            LoadLevel();
        }

        private void Update()
        {
            if (_printPlayerProgress)
                print(_progressService.Progress);

            float deltaTime = Time.deltaTime;

            _gameTimer?.Tick(deltaTime);
        }

        //private void OnApplicationQuit() => _saveLoadService.SaveProgress();

        public void ExitGame() => Application.Quit();

        /// <summary>
        /// go to next level
        /// </summary>
        public void GoToNextStage()
        {
            ClearScene();
            LoadLevel();
        }

        public void RestartStage()
        {
            GoToNextStage(); // no, really restart this scene ))))
        }

        /// <summary>
        /// go to stage after bought purchase or not
        /// </summary>
        public void GoToCurrentStage()
        {
            _pauseManager.UnPause();
        }

        private void LoadLevel()
        {
            // load level state
            string curLevelName = _progressService.Progress.WorldData.LevelNumber.ToString();
            _currentLevelData = null;
            foreach (var level in _gameSettings.LevelsDatas)
            {
                if (level.SceneName == curLevelName)
                {
                    _currentLevelData = level;
                    break;
                }
            }
            if (_currentLevelData == null)
            {
                _progressService.Progress.WorldData.LevelNumber--;
                _currentLevelData = _gameSettings.LevelsDatas[_progressService.Progress.WorldData.LevelNumber - 1];
                Debug.LogWarning($"Level {curLevelName} not found, loading {_currentLevelData.SceneName}");
            }

            string loadingScene = _startSceneName ?? _currentLevelData.SceneName;
            _sceneLoader.Load(loadingScene, OnLoaded);
        }

        private void OnLoaded()
        {
            _gameTimer.Restart(_currentLevelData.Duration);

            _levelManager = FindObjectOfType<LevelManagerBase>();
            _levelManager.Initialize(_currentLevelData.EnemyStats, _inputService, _progressService, _gameTimer);
            _levelManager.PlayerWin += OnPlayerWin;
            _levelManager.PlayerLose += OnPlayerLose;
            _pauseManager.Add(_levelManager);

            _player = _levelManager.Player;
            _pauseManager.Add(_player);

            _cameraParent.Construct(_player.transform);

            _pauseManager.Pause();
            _uIMediator.Init(_sceneLoader.CurrentSceneName, _levelManager.GameMode, _levelManager.EnemiesCount);
            _uIMediator.ShowAbilityPurchaseScreen();
        }

        private void OnPlayerWin()
        {
            _pauseManager.Pause();

            _levelManager.PlayerWin -= OnPlayerWin;
            _levelManager.PlayerLose -= OnPlayerLose;

            _gameTimer.Stop();
            _uIMediator.ShowWinScreen(_currentLevelData.Reward);

            _progressService.Progress.PlayerLoot.Add(_currentLevelData.Reward);

            _progressService.Progress.WorldData.LevelNumber++;

            if (DebugMode == false)
                _saveLoadService.SaveProgress();
        }

        private void OnPlayerLose()
        {
            _pauseManager.Pause();

            _levelManager.PlayerWin -= OnPlayerWin;
            _levelManager.PlayerLose -= OnPlayerLose;

            _gameTimer.Stop();
            _uIMediator.ShowLoseScreen(_currentLevelData.Reward / 2);

            _progressService.Progress.PlayerLoot.Add(_currentLevelData.Reward / 2);

            if (DebugMode == false)
                _saveLoadService.SaveProgress();
        }

        private void ClearScene()
        {
            _pauseManager.Remove(_player);
            _pauseManager.Remove(_levelManager);

            _levelManager.ClearScene();
        }

        private void RegisterServices()
        {
            _inputService = GetInputService();
            _progressService = new PersistentProgressService();
            _saveLoadService = new PlayerPrefsSaveLoadService(_progressService);
            //_saveLoadService = new JsonFileSaveLoadService(_progressService);

            _sceneLoader = new SceneLoader(this);
            _pauseManager = new PauseManager();
        }

        private IInputService GetInputService()
        {
            return new MobileInputService(_joystick);
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =
              _saveLoadService.LoadProgressOrNull()
              ?? NewProgress();

            _progressService.Progress.PlayerGranmaStats.AbilityCost = _gameSettings.AbilityCost;
            _progressService.Progress.PlayerBullyStats.AbilityCost = _gameSettings.AbilityCost;
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(defaultLevelNumber: 1);

            //progress.

            return progress;
        }
    }
}