using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Items
{
    public class SpawnPotion
    {
        private readonly NetworkRunner _runner;
        private readonly Potion _prefab;

        public SpawnPotion(NetworkRunner runner, Potion prefab)
        {
            _runner = runner;
            _prefab = prefab;
        }
        
        public void Spawn(Vector2  position)
        {
            if (_prefab == null)
                return;
            
            if (!_runner.IsServer)
                return;
            
            _runner.Spawn(_prefab,  position, Quaternion.identity);
        }
    }
}