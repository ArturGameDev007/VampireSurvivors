using System;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.Infrastructure.Player.Shoot;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class GameFactory : IGameFactory
    {
        public event Action<Character> OnCharacterCreated;

        private readonly Bullet _bulletprefab;
        private readonly EnemyRegistry _enemyRegistry;
        private readonly PlayerRegistry _playerRegistry;
        private readonly BonusApplier _bonusApplier;

        public GameFactory( Bullet prefab, EnemyRegistry enemyRegistry,
            PlayerRegistry playerRegistry, BonusApplier bonusApplier)
        {
            _bulletprefab = prefab;
            _enemyRegistry = enemyRegistry;
            _playerRegistry = playerRegistry;
            _bonusApplier = bonusApplier;
        }

        public void CreatePlayer(NetworkRunner runner, Character prefab, PlayerRef player)
        {
            if (prefab == null)
                throw new MissingReferenceException("Префаб игрока не передан.");

            float distanceBetweenPlayers = 3f;

            Vector3 spawnPosition = new Vector3(player.PlayerId * distanceBetweenPlayers, 0f, 0f);

            Character newPlayer = runner.Spawn(prefab, spawnPosition, Quaternion.identity, player);

            newPlayer.Initialize(_playerRegistry, _bonusApplier);
            _playerRegistry.Register(newPlayer);

            if (player == runner.LocalPlayer)
            {
                OnCharacterCreated?.Invoke(newPlayer);
            }
            else
            {
                newPlayer.gameObject.name = $"[REMOTE_CLIENT_{player.PlayerId}]";
            }

            if (newPlayer.TryGetComponent(out ShootController controller))
            {
                var personalBullets = new GenerateBullets(runner, _bulletprefab, _enemyRegistry);
                controller.Initialize(personalBullets);
            }
        }
    }
}