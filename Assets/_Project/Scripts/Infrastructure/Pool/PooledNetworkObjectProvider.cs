using System.Collections.Generic;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Items;
using _Project.Scripts.Infrastructure.Player.Shoot;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Pool
{
    public class PooledNetworkObjectProvider : NetworkObjectProviderDefault
    {
        private readonly Dictionary<NetworkPrefabId, Queue<NetworkObject>> _pools = new();

        private Transform _rootPoolContainer;
        private Transform _enemyContainer;
        private Transform _bulletContainer;
        private Transform _potionContainer;
        private Transform _diamondContainer;

        private void Awake()
        {
            _rootPoolContainer = new GameObject("PoolRoot").transform;
            _rootPoolContainer.SetParent(transform);
            
            _enemyContainer = new GameObject("Enemy").transform;
            _enemyContainer.SetParent(_rootPoolContainer);
            
            _bulletContainer = new GameObject("Bullet").transform;
            _bulletContainer.SetParent(_rootPoolContainer);
            
            _potionContainer = new GameObject("Potion").transform;
            _potionContainer.SetParent(_enemyContainer);
            
            _diamondContainer = new GameObject("Diamond").transform;
            _diamondContainer.SetParent(_enemyContainer);
        }

        public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context,
            out NetworkObject instance)
        {
            instance = null;

            NetworkObject prefab;
            
            try
            {
                prefab = runner.Prefabs.Load(context.PrefabId, isSynchronous: context.IsSynchronous);
            }
            catch
            {
                return NetworkObjectAcquireResult.Failed;
            }

            if (!prefab)
                return NetworkObjectAcquireResult.Retry;

            if (!_pools.TryGetValue(context.PrefabId, out var pool))
            {
                pool = new Queue<NetworkObject>();
                _pools[context.PrefabId] = pool;
            }

            if (pool.Count > 0)
            {
                instance = pool.Dequeue();
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = Instantiate(prefab);
            }

            instance.transform.SetParent(GetTargetContainer(instance));

            if (context.DontDestroyOnLoad)
                runner.MakeDontDestroyOnLoad(instance.gameObject);

            return NetworkObjectAcquireResult.Success;
        }

        public override void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
        {
            NetworkObject instance = context.Object;
            
            if (instance == null)
                return;
            
            if (!context.TypeId.IsPrefab)
            {
                base.ReleaseInstance(runner, context);
                return;
            }
            
            if (context.IsBeingDestroyed)
            {
                base.ReleaseInstance(runner, context);
                return;
            }
            
            NetworkPrefabId prefabId = context.TypeId.AsPrefabId;

            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_rootPoolContainer);

            if (!_pools.ContainsKey(prefabId))
                _pools[prefabId] = new Queue<NetworkObject>();

            int maxSize = GetSizePool(instance);

            if (_pools[prefabId].Count >= maxSize)
            {
                base.ReleaseInstance(runner, context);
                return;
            }

            _pools[prefabId].Enqueue(instance);
        }

        private int GetSizePool(NetworkObject instance)
        {
            int maxSizePoolEnemy = 20;
            int maxSizePoolBullet = 15;
            int maxSizePoolPotion = 10;
            int maxSizePoolDiamond = 10;
            int minSize = 0;

            return instance switch
            {
                _ when instance.TryGetComponent(out Enemy _) => maxSizePoolEnemy,
                _ when instance.TryGetComponent(out Bullet _) => maxSizePoolBullet,
                _ when instance.TryGetComponent(out Potion _) => maxSizePoolPotion,
                _ when instance.TryGetComponent(out Diamond _) => maxSizePoolDiamond,
                _ => minSize
            };
        }

        private Transform GetTargetContainer(NetworkObject objectType)
        {
            if (objectType == null)
                return null;

            return objectType switch
            {
                _ when objectType.TryGetComponent(out Enemy _) => _enemyContainer,
                _ when objectType.TryGetComponent(out Bullet _) => _bulletContainer,
                _ when objectType.TryGetComponent(out Potion _) => _potionContainer,
                _ when objectType.TryGetComponent(out Diamond _) => _diamondContainer,
                _ => null
            };
        }
    }
}