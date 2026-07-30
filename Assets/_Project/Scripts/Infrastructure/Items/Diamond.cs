using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Items
{
    public class Diamond : NetworkBehaviour
    {
        [SerializeField] private float _experienceAmount = 20f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!HasStateAuthority)
                return;
            
            if (other.TryGetComponent(out Character character))
            {
                character.Experience(_experienceAmount);
                Runner.Despawn(Object);
            }
        }
    }
}