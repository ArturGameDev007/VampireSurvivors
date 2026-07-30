using _Project.Scripts.Infrastructure.Pool;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services.PhotonFusion
{
    public class FusionConnector : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _sessionPrefab;

        private NetworkRunner _activeRunnerInstance;

        public NetworkRunner ActiveRunnerInstance => _activeRunnerInstance;
        
        private void Awake()
        {
            Application.runInBackground = true;
        }

        public async UniTask StartFusionSession(GameMode mode, string roomName)
        {
            if (_sessionPrefab == null)
                return;

            _activeRunnerInstance = Instantiate(_sessionPrefab);
            _activeRunnerInstance.name = "PhotonSession";
            _activeRunnerInstance.ProvideInput = true;

            DontDestroyOnLoad(_activeRunnerInstance);

            var sceneManager = _activeRunnerInstance.GetComponent<NetworkSceneManagerDefault>();

            if (sceneManager == null)
                return;
            
            var objectProvider = _activeRunnerInstance.GetComponent<PooledNetworkObjectProvider>();
            
            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                SceneManager = sceneManager,
                ObjectProvider = objectProvider
            };

            if (mode == GameMode.Host)
            {
                int gameplaySceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                startGameArgs.Scene = SceneRef.FromIndex(gameplaySceneIndex);
            }

            await _activeRunnerInstance.StartGame(startGameArgs);

            Debug.Log($"Сеть запущена в режиме: {mode}. Комната: {roomName}");
        }

        private void OnApplicationQuit()
        {
            if (_sessionPrefab != null && _sessionPrefab.IsRunning)
            {
                _sessionPrefab.Shutdown();
            }
        }
    }
}