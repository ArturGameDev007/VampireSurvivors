using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services.PhotonFusion
{
    public class FusionConnector : MonoBehaviour
    {
        [SerializeField] private NetworkRunner _sessionPrefab;

        private void Awake()
        {
            Application.runInBackground = true;
        }
        
        public async UniTaskVoid StartFusionSession(GameMode mode, string roomName)
        {
            if (_sessionPrefab == null)
                return;

            NetworkRunner currentRunner = Instantiate(_sessionPrefab);

            currentRunner.name = "PhotonSession";
            currentRunner.ProvideInput = true;
            
            DontDestroyOnLoad(currentRunner);

            var sceneManager = currentRunner.GetComponent<NetworkSceneManagerDefault>();

            if (sceneManager == null)
                sceneManager = currentRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();

            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                SceneManager = sceneManager
            };

            if (mode == GameMode.Host)
            {
                int gameplaySceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                startGameArgs.Scene = SceneRef.FromIndex(gameplaySceneIndex);
            }
            else if (mode == GameMode.Client)
            {
                startGameArgs.Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            }

            await currentRunner.StartGame(startGameArgs);

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