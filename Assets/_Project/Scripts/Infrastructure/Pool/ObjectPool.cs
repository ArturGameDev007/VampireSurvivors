// using System.Collections.Generic;
// using _Project.Scripts.Infrastructure.EnemyInformation;
// using _Project.Scripts.Infrastructure.Player.Shoot;
// using UnityEngine;
//
// namespace _Project.Scripts.Infrastructure.Pool
// {
//     public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
//     {
//         private Queue<T> _pool;
//         private Transform _container;
//
//         private int _initialPoolSize;
//         private bool _isFilled;
//
//         public T Prefab { get; private set; }
//
//         // public ObjectPool(T prefab, Transform parent)
//         // {
//         //     // _runner = runner;
//         //     Prefab = prefab;
//         //     _container = parent;
//         //     _initialPoolSize = GetSizePool();
//         //     _pool = new Queue<T>(_initialPoolSize);
//         //
//         //     // AddObject(_initialPoolSize);
//         // }
//
//         public T GetObject(T prefab)
//         {
//             if (prefab == null)
//                 return null;
//
//             if (!_isFilled)
//             {
//                 _isFilled = true;
//                 AddObject(_initialPoolSize);
//             }
//
//             if (!_pool.TryDequeue(out T objectType))
//                 objectType = CreateNewObject(true);
//             
//             objectType.gameObject.SetActive(true);
//
//
//             return objectType;
//         }
//
//         public void ReturnObject(T typeObject)
//         {
//             if (typeObject == null)
//                 return;
//
//             typeObject.transform.SetParent(_container);
//             typeObject.gameObject.SetActive(false);
//
//             _pool.Enqueue(typeObject);
//         }
//
//         private T CreateNewObject(bool isActive)
//         {
//             Vector3 targetPosition = new Vector3(0, -100f, 0);
//
//             T newObject = Instantiate(Prefab, targetPosition, Quaternion.identity);
//
//             if (newObject != null)
//             {
//                 newObject.transform.SetParent(_container);
//                 newObject.gameObject.SetActive(isActive);
//
//                 if (!isActive)
//                     _pool.Enqueue(newObject);
//             }
//
//             return newObject;
//         }
//
//         private void AddObject(int count)
//         {
//             for (int i = 0; i < count; i++)
//             {
//                 CreateNewObject(false);
//             }
//         }
//     }
// }