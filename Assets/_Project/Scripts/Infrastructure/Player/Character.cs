using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public class Character : NetworkBehaviour
    {
        private Camera _mainCamera;
        
        public override void Spawned()
        {
            if (!HasInputAuthority)
                return;
            
            _mainCamera = Camera.main;
                
            if (_mainCamera != null && _mainCamera.TryGetComponent(out CameraFollow cameraFollow))
            {
                cameraFollow.Initialize(this);
            }
        }
    }
}