using System;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Items;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.Infrastructure.Player.Shoot;
using _Project.Scripts.Services.PhotonFusion;
using _Project.Scripts.UI.CharacterMovementController;
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
        
        [Header("Items")]
        [SerializeField] private Potion _potionPrefab;
        [SerializeField] private Diamond _diamondPrefab;

        private NetworkRunner _runner;
        private GenerateEnemies _generateEnemies;
        private GenerateBullets _generateBullets;

        public GameManager Compose(FusionConnector fusionConnector)
        {
            _runner = fusionConnector.ActiveRunnerInstance;

            IPlayerProvider playerProvider = new PlayerProvider();
            PlayerRegistry playerRegistry = new PlayerRegistry();
            EnemyRegistry enemyRegistry = new EnemyRegistry();
            SpawnPotion spawnPotion = new SpawnPotion(_runner, _potionPrefab);
            SpawnDiamond spawnDiamond = new SpawnDiamond(_runner, _diamondPrefab);
            BonusApplier bonusApplier = new BonusApplier();
            
            IGameFactory gameFactory = new GameFactory(_bulletPrefab, enemyRegistry, playerRegistry, bonusApplier);
            _generateEnemies = new GenerateEnemies(_prefabEnemies, _runner, playerProvider, playerRegistry, enemyRegistry, spawnPotion, spawnDiamond);
            EnemySpawnController enemySpawnController = new EnemySpawnController(gameFactory, _generateEnemies);

            return new GameManager(_runner, gameFactory, _characterPrefab, _generateEnemies, enemySpawnController, playerRegistry, _joystickController);
        }
    }
}