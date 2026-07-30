using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Items
{
    public class Potion : NetworkBehaviour
    {
        [SerializeField] private float _healAmount = 5f;
            
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!HasStateAuthority)
                return;
            
            if (other.TryGetComponent(out Character character))
            {
                character.Heal(_healAmount);
                Runner.Despawn(Object);
            }
        }
    }
}