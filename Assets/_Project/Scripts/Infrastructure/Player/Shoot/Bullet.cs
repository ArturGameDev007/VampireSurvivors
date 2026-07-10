using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.Infrastructure.Pool;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player.Shoot
{
    public class Bullet : NetworkBehaviour
    {
        // private NetworkPool<Bullet> _pool;

        // public void Init(NetworkPool<Bullet> pool)
        // {
        //     _pool = pool;
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Runner == null || !Runner.IsServer)
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out Enemy _))
            {
                // _pool.ReturnObject(this);
                Runner.Despawn(Object);
            }
        }
    }
}