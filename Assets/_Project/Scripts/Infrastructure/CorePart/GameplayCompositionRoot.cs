using System;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.Infrastructure.Player.Shoot;
using _Project.Scripts.Infrastructure.Pool;
using _Project.Scripts.Services.PhotonFusion;
using _Project.Scripts.UI.PlayerMovement;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.CorePart
{
    [Serializable]
    public class GameplayCompositionRoot
    {
        [Header("Settings Player")]
        [SerializeField] private Character _characterPrefab;
        [SerializeField] private FixedJoystickController _joystickController;

        [Header("Enemy")]
        [SerializeField] private Enemy[] _prefabEnemies;
        [SerializeField] private Bullet _bulletPrefab;

        private NetworkRunner _runner;
        private GenerateEnemies _generateEnemies;

        private GenerateBullets _generateBullets;
        // private NetworkPool<Enemy>[] _enemyPools;
        // private NetworkPool<Bullet> _bulletPool;

        public GameManager Compose(FusionConnector fusionConnector)
        {
            _runner = fusionConnector.ActiveRunnerInstance;

            IPlayerProvider playerProvider = new PlayerProvider();
            EnemyRegistry enemyRegistry = new EnemyRegistry();

            // SetupPools(out Transform enemyContainer, out Transform shootsContainer);

            // var poolProvider = fusionConnector.ObjectProvider;
            //
            // if (poolProvider != null)
            // {
            //     poolProvider.InitializeContainers(enemyContainer, shootsContainer);
            // }

            IGameFactory gameFactory = new GameFactory(_bulletPrefab, enemyRegistry);
            _generateEnemies = new GenerateEnemies(_prefabEnemies, _runner, playerProvider, enemyRegistry);
            // GenerateBullets generateBullets = new GenerateBullets(_runner, _bulletPool, enemyRegistry);
            EnemySpawnController enemySpawnController = new EnemySpawnController(gameFactory, _generateEnemies);

            return new GameManager(_runner, gameFactory, _characterPrefab, _generateEnemies, enemySpawnController,
                _joystickController);
        }

        // private void SetupPools(out Transform enemyContainer, out Transform shootsContainer)
        // {
        //     Transform rootPools = new GameObject("RootPools").transform;
        //
        //     enemyContainer = new GameObject("Enemy_Pool_Category").transform;
        //     enemyContainer.parent = rootPools;
        //
        //     shootsContainer = new GameObject("Shoot_Pool_Category").transform;
        //     shootsContainer.parent = rootPools;
        // }

        // private void PoolEnemy(Transform container)
        // {
        //     _enemyPools = new NetworkPool<Enemy>[_prefabEnemies.Length];
        //     
        //     for (int i = 0; i < _prefabEnemies.Length; i++)
        //     {
        //         var prefab = _prefabEnemies[i];
        //         _enemyPools[i] = new NetworkPool<Enemy>(prefab, container);
        //     }
        // }
        //
        // private void PoolBullet(Transform container)
        // {
        //     _bulletPool = new NetworkPool<Bullet>(_bulletPrefab, container);
        // }
    }
}