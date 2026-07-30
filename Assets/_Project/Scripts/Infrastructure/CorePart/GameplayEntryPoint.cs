using _Project.Scripts.Services.PhotonFusion;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.CorePart
{
    public class GameplayEntryPoint : NetworkBehaviour
    {
        [SerializeField] private GameplayCompositionRoot _gameplayCompositionRoot;
        [SerializeField] private FusionConnector _fusionConnector;

        private GameManager _gameManager;
        private NetworkRunner _runner;

        public override void Spawned()
        {
            _gameManager = _gameplayCompositionRoot.Compose(_fusionConnector);

            _runner = _fusionConnector.ActiveRunnerInstance;

            if (_runner != null)
                _runner.AddCallbacks(_gameManager);

            _gameManager.Initialize();
        }

        public override void FixedUpdateNetwork()
        {
            _gameManager?.Tick();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (_runner != null)
                _runner.RemoveCallbacks(_gameManager);

            _gameManager.Destroy();
        }
    }
}