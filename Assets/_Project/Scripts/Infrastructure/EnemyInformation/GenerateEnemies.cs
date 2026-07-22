using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.Infrastructure.Pool;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    public class GenerateEnemies
    {
        // private readonly NetworkPool<Enemy>[] _enemyPool;
        private readonly Enemy[] _prefab;
        private readonly NetworkRunner _runner;
        private readonly IPlayerProvider _playerProvider;
        private readonly PlayerRegistry  _playerRegistry;
        private readonly EnemyRegistry _enemyRegistry;

        private float _spawnTimer;
        private float _delay = 0.5f;
        private float _minRadius = 15f;
        private float _maxRadius = 16f;

        private bool _isActiveGame = true;

        public GenerateEnemies(Enemy[] prefab, NetworkRunner runner,
            IPlayerProvider playerProvider, PlayerRegistry playerRegistry, EnemyRegistry enemyRegistry)
        {
            // _enemyPool = pool;
            _prefab = prefab;
            _runner = runner;
            _playerProvider = playerProvider;
            _playerRegistry = playerRegistry;
            _enemyRegistry = enemyRegistry;
        }

        public void SetPlayerProvider(Character playerTransform)
        {
            _playerProvider.SetPlayer(playerTransform);
        }

        public void Process()
        {
            if (!_runner.IsServer)
                return;

            if (!_isActiveGame)
                return;

            float minTimerThreshold = 0f;

            _spawnTimer -= _runner.DeltaTime;

            while (_spawnTimer <= minTimerThreshold)
            {
                SpawnEnemy();
                _spawnTimer += _delay;
            }
        }

        private void SpawnEnemy()
        {
            if (_prefab.Length == 0)
                return;

            var randomEnemy = Random.Range(0, _prefab.Length);

            Vector3 spawnPosition = GetRandomPoint();

            // Enemy enemySpawned = _enemyPool[randomEnemy].GetObject(_runner);
            Enemy enemySpawned = _runner.Spawn(_prefab[randomEnemy], spawnPosition, Quaternion.identity);

            if (enemySpawned == null)
                return;
            
            // enemySpawned.transform.position = spawnPosition;

            _enemyRegistry.Add(enemySpawned);

            // enemySpawned.Initialize(_enemyPool[randomEnemy], _enemyRegistry);
            enemySpawned.Initialize(_enemyRegistry);

            if (enemySpawned.TryGetComponent(out EnemyController enemyController))
                enemyController.Initialize(_playerRegistry);
        }

        private Vector3 GetRandomPoint()
        {
            float spawnRadius = Random.Range(_minRadius, _maxRadius);

            Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = new Vector3(randomDirection.x, randomDirection.y, 0f);

            return spawnPosition;
        }
    }
}