using System;
using _Project.Scripts.Infrastructure.Player;
using _Project.Scripts.Infrastructure.Player.Shoot;
using _Project.Scripts.Infrastructure.Pool;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    public class Enemy : NetworkBehaviour
    {
        // private NetworkPool<Enemy> _pool;
        private EnemyRegistry _enemyRegistry;
        [field: SerializeField] public float Damage { get; private set; } = 10f;
        
        public bool IsTargeted { get; set; }

        public void Initialize(EnemyRegistry enemyRegistry)
        {
            // _pool = pool;
            _enemyRegistry = enemyRegistry;
            
            IsTargeted = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Runner == null || !Runner.IsServer)
            {
                return;
            }

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
            
            // _pool.ReturnObject(this);
            
            Runner.Despawn(Object);
        }
    }
}