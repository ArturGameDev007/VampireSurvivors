using _Project.Scripts.Infrastructure.EnemyInformation;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player.Shoot
{
    public class Bullet : NetworkBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Runner == null || !Runner.IsServer)
                return;

            if (other.gameObject.TryGetComponent(out Enemy _))
            {
                Runner.Despawn(Object);
            }
        }
    }
}