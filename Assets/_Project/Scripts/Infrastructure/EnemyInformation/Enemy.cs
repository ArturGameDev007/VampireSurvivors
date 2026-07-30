using _Project.Scripts.Infrastructure.Items;
using _Project.Scripts.Infrastructure.Player.Shoot;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    public class Enemy : NetworkBehaviour
    {
        private EnemyRegistry _enemyRegistry;
        private SpawnPotion _spawnPotion;
        private SpawnDiamond _spawnDiamond;

        [field: SerializeField] public float Damage { get; private set; } = 10f;

        public bool IsTargeted { get; set; }

        public void Initialize(EnemyRegistry enemyRegistry, SpawnPotion spawnPotion,  SpawnDiamond spawnDiamond)
        {
            _enemyRegistry = enemyRegistry;
            _spawnPotion = spawnPotion;
            _spawnDiamond = spawnDiamond;

            IsTargeted = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Runner == null || !Runner.IsServer)
                return;
            
            if (other.TryGetComponent(out Bullet _))
            {
                Kill();
            }
        }

        private void Kill()
        {
            IsTargeted = false;

            if (_enemyRegistry != null)
                _enemyRegistry.Remove(this);

            _spawnPotion?.Spawn(transform.position);
            _spawnDiamond?.Spawn(transform.position);
            
            Runner.Despawn(Object);
        }
    }
}