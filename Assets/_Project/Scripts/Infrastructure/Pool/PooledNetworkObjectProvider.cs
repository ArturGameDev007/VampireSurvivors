// using System;
// using System.Collections.Generic;
// using _Project.Scripts.Infrastructure.EnemyInformation;
// using _Project.Scripts.Infrastructure.Player.Shoot;
// using Fusion;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace _Project.Scripts.Infrastructure.Pool
// {
//     public class PooledNetworkObjectProvider : NetworkObjectProviderDefault
//     {
//         // private NetworkObjectPool _pool;
//
//         private readonly Dictionary<NetworkPrefabId, Queue<NetworkObject>> _pools = new();
//         // private Transform _rootPoolContainer;
//
//         // private Transform _enemyContainer;
//         // private Transform _bulletContainer;
//
//         // public void InitializeContainers(Transform enemyContainer, Transform bulletContainer)
//         // {
//         //     _enemyContainer = enemyContainer;
//         //     _bulletContainer = bulletContainer;
//         // }
//
//         protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab)
//         {
//             NetworkPrefabId prefabId = prefab.NetworkTypeId.AsPrefabId;
//
//             if (!_pools.ContainsKey(prefabId))
//                 _pools[prefabId] = new Queue<NetworkObject>();
//
//             NetworkObject result = null;
//
//             if (_pools[prefabId].Count > 0)
//             {
//                 result = _pools[prefabId].Dequeue();
//             }
//
//             if (result == null)
//             {
//                 result = Instantiate(prefab);
//                 Debug.Log("fff");
//             }
//             else
//             {
//                 result.gameObject.SetActive(true);
//             }
//             //
//             // if (result != null)
//             // {
//             //     result.transform.SetParent(targetContainer);
//             //     // Transform targetContainer = GetTargetContainer(result);
//             //
//             //     // if (targetContainer != null)
//             // }
//
//             return result;
//         }
//
//         protected override void DestroyPrefabInstance(NetworkRunner runner, NetworkPrefabId prefabId,
//             NetworkObject instance)
//         {
//             if (instance == null)
//             {
//                 return;
//             }
//
//             instance.gameObject.SetActive(false);
//             instance.transform.SetParent(null);
//
//
//             // Transform targetContainer = GetTargetContainer(instance);
//             // if (targetContainer != null)
//
//
//             if (!_pools.ContainsKey(prefabId))
//             {
//                 _pools[prefabId] = new Queue<NetworkObject>();
//             }
//
//             _pools[prefabId].Enqueue(instance);
//         }
//
//         // private Transform GetTargetContainer(NetworkObject objectType)
//         // {
//         //     if (objectType == null)
//         //         return null;
//         //
//         //     if (objectType.TryGetComponent(out Bullet _))
//         //         return _bulletContainer;
//         //
//         //     if (objectType.TryGetComponent(out Enemy _))
//         //         return _enemyContainer;
//         //
//         //     return null;
//         // }
//     }
// }