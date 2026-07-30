using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Items
{
    public class SpawnDiamond
    {
        private readonly NetworkRunner _runner;
        private readonly Diamond _prefab;

        public SpawnDiamond(NetworkRunner runner, Diamond prefab)
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
            
            _runner.Spawn(_prefab, position,  Quaternion.identity);
        }
    }
}