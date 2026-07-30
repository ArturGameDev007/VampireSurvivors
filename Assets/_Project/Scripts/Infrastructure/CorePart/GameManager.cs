using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.UI.CharacterMovementController;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.CorePart
{
    public class GameManager : INetworkRunnerCallbacks
    {
        private readonly NetworkRunner _runner;
        private readonly IGameFactory _gameFactory;
        private readonly Character _character;
        private readonly GenerateEnemies _generateEnemies;
        private readonly EnemySpawnController _spawnController;
        private readonly PlayerRegistry _playerRegistry;
        private readonly IFixedJoystickController _joystickController;

        private bool _isInitialized;
        private bool _isReturnToLobby;

        public GameManager(NetworkRunner runner, IGameFactory gameFactory, Character character,
            GenerateEnemies generateEnemies, EnemySpawnController spawnController, PlayerRegistry playerRegistry, IFixedJoystickController joystickController)
        {
            _runner = runner;
            _gameFactory = gameFactory;
            _character = character;
            _generateEnemies = generateEnemies;
            _spawnController = spawnController;
            _playerRegistry = playerRegistry;
            _joystickController = joystickController;
        }

        public void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _spawnController?.Enable();
        }

        public void Tick()
        {
            _generateEnemies?.Process();
            
            CheckNextLevel();
        }

        public void Destroy()
        {
            _isInitialized = false;

            if (_spawnController != null)
            {
                _spawnController.Disable();
            }
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                _gameFactory.CreatePlayer(_runner, _character, player);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            ReturnToLobbyOnce();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            ReturnToLobbyOnce();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            InputController controller = default;
            
            controller.GetJoystick(_joystickController);
            input.Set(controller);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        private void CheckNextLevel()
        {
            foreach (var player in _playerRegistry.AddPlayers)
            {
                if (player == null)
                    continue;

                if (player.CurrentExperience < player.MaxExperience)
                    continue;

                int nextLevel = 1;

                player.NextLevel(player.CurrentLevel + nextLevel);
            }
        }

        private void ReturnToLobbyOnce()
        {
            if (_isReturnToLobby)
                return;

            _isReturnToLobby = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}