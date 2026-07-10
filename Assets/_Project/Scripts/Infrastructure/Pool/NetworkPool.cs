using System.Collections.Generic;
using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Player.Shoot;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Pool
{
    public class NetworkPool<T> where T : NetworkBehaviour
    {
        // private NetworkRunner _runner;
        private Queue<T> _pool;
        private Transform _container;

        private int _initialPoolSize;
        private bool _isFilled;

        public T Prefab { get; set; }

        public NetworkPool(T prefab, Transform parent)
        {
            // _runner = runner;
            Prefab = prefab;
            _container = parent;
            _initialPoolSize = GetSizePool();
            _pool = new Queue<T>(_initialPoolSize);

            // AddObject(_initialPoolSize);
        }

        public T GetObject(NetworkRunner runner)
        {
            if (Prefab == null)
                return null;

            if (!_isFilled)
            {
                _isFilled = true;
                AddObject(runner, _initialPoolSize);
            }

            if (!_pool.TryDequeue(out T objectType))
                objectType = CreateNewObject(runner,true);
            
            objectType.gameObject.SetActive(true);


            return objectType;
        }

        public void ReturnObject(T typeObject)
        {
            if (typeObject == null)
                return;

            typeObject.transform.SetParent(_container);

            typeObject.gameObject.SetActive(false);

            _pool.Enqueue(typeObject);
        }

        private T CreateNewObject(NetworkRunner runner, bool isActive)
        {
            Vector3 targetPosition = new Vector3(0, -100f, 0);

            T newObject = runner.Spawn(Prefab, targetPosition, Quaternion.identity);


            if (newObject != null)
            {
                newObject.transform.SetParent(_container);
                newObject.gameObject.SetActive(isActive);

                if (!isActive)
                    _pool.Enqueue(newObject);
            }

            return newObject;
        }

        private int GetSizePool()
        {
            var type = typeof(T);

            int maxSizePoolEnemy = 10;
            int maxSizePoolBullet = 15;

            int minSize = 0;

            if (type == typeof(Enemy))
                return maxSizePoolEnemy;

            if (type == typeof(Bullet))
                return maxSizePoolBullet;

            return minSize;
        }

        private void AddObject(NetworkRunner runner, int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateNewObject(runner,false);
            }
        }
    }
}