using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Pool;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player.Shoot
{
    public class GenerateBullets
    {
        private readonly NetworkRunner _runner;
        // private readonly NetworkPool<Bullet> _bulletPool;
        // private readonly ObjectPool<Bullet> _bulletPool;
        private readonly Bullet _prefab;
        private readonly EnemyRegistry _enemyRegistry;

        private float _spawnTimer;
        private float _delay = 1.5f;
        private float _attackRadius = 10f;

        public GenerateBullets(NetworkRunner runner, Bullet prefab, EnemyRegistry enemyRegistry)
        {
            _runner = runner;
            _prefab = prefab;
            _enemyRegistry = enemyRegistry;
        }

        public void Process(Transform point)
        {
            if (!_runner.IsServer) 
                return;

            float minTimerThreshold = 0f;

            _spawnTimer -= _runner.DeltaTime;

            if (_spawnTimer <= minTimerThreshold)
            {
                Enemy closestEnemy = GetClosestEnemy(point.position);
            
                if (closestEnemy != null)
                {
                    SpawnShoot(point, closestEnemy);
                    _spawnTimer = _delay;
                }
            }
        }

        private void SpawnShoot(Transform shootPoint, Enemy enemyTarget)
        {
            // Bullet bullet = _bulletPool.GetObject(_runner);
            Bullet bullet = _runner.Spawn(_prefab, shootPoint.position, Quaternion.identity);

            if (bullet == null)
                return;

            bullet.transform.position = shootPoint.position;
            // bullet.Init(_bulletPool);
            
            enemyTarget.IsTargeted = true;

            if (bullet.TryGetComponent(out BulletMovement bulletMovement))
            {
                bulletMovement.Initialize(enemyTarget);
            }
        }

        private Enemy GetClosestEnemy(Vector2 playerPosition)
        {
            if (_enemyRegistry == null)
                return null;

            var activeEnemies = _enemyRegistry.ActiveEnemies;

            if (activeEnemies == null || activeEnemies.Count == 0)
                return null;

            Enemy closestEnemy = null;
            float minDistance = _attackRadius;

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                var enemy = activeEnemies[i];

                if (enemy != null && !enemy.IsTargeted)
                {
                    Vector2 actualEnemyPosition = enemy.transform.position;

                    float directionToEnemy = (actualEnemyPosition - playerPosition).magnitude;

                    if (directionToEnemy <= minDistance)
                    {
                        minDistance = directionToEnemy;
                        closestEnemy = enemy;
                    }
                }
            }

            return closestEnemy;
        }
    }
}