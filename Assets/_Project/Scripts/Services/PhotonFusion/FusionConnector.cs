using _Project.Scripts.Infrastructure.Pool;
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
        
        // public PooledNetworkObjectProvider ObjectProvider { get; private set; }

        private void Awake()
        {
            Application.runInBackground = true;
        }

        public void StartFusionSession(GameMode mode, string roomName)
        {
            if (_sessionPrefab == null)
                return;

            _activeRunnerInstance = Instantiate(_sessionPrefab);
            _activeRunnerInstance.name = "PhotonSession";
            _activeRunnerInstance.ProvideInput = true;

            DontDestroyOnLoad(_activeRunnerInstance);

            var sceneManager = _activeRunnerInstance.GetComponent<NetworkSceneManagerDefault>();

            if (sceneManager == null)
                sceneManager = _activeRunnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>();
            
            // ObjectProvider = new PooledNetworkObjectProvider();
            
            // var provider=_activeRunnerInstance.gameObject.AddComponent<NetworkObjectProviderDefault>();

            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                SceneManager = sceneManager
                // ObjectProvider =  provider
                
            };

            if (mode == GameMode.Host)
            {
                int gameplaySceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                startGameArgs.Scene = SceneRef.FromIndex(gameplaySceneIndex);
            }
            // else if (mode == GameMode.Client)
            // {
            //     startGameArgs.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            // }

            _activeRunnerInstance.StartGame(startGameArgs);

            Debug.Log($"Сеть запущена в режиме: {mode}. Комната: {roomName}");
        }

        // private void OnApplicationQuit()
        // {
        //     if (_sessionPrefab != null && _sessionPrefab.IsRunning)
        //     {
        //         _sessionPrefab.Shutdown();
        //     }
        // }
    }
}