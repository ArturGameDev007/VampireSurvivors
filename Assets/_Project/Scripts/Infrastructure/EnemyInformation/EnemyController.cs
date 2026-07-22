using _Project.Scripts.Infrastructure.Player;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NetworkRigidbody2D))]
    public class EnemyController : NetworkBehaviour
    {
        [SerializeField] private float _speed;

        private IPlayerProvider _playerProvider;
        private PlayerRegistry _playerRegistry;
        private Rigidbody2D _rigidbody2D;

        private bool _canMove = true;

        [Networked] public Character Target { get; private set; }

        public void Initialize(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        // public void Initialize(IPlayerProvider player)
        // {
        //     _playerProvider = player;
        //
        //     if (_playerProvider != null && _playerProvider.PlayerTransform != null)
        //         Target = _playerProvider.PlayerTransform;
        // }

        public override void Spawned()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Runner.IsServer || _playerRegistry == null)
            {
                return;
            }
            
            Target = _playerRegistry.GetClosestTo(_rigidbody2D.position);
            
            if (!_canMove || Target == null)
                return;

            Move();
        }

        private void Move()
        {
            Vector2 currentPosition = _rigidbody2D.position;
            Vector2 target = Target.transform.position;

            Vector2 nextPosition = Vector2.MoveTowards(currentPosition, target, _speed * Runner.DeltaTime);

            _rigidbody2D.MovePosition(nextPosition);
            // Vector2 nextPosition = (target - currentPosition).normalized;

            // _rigidbody2D.velocity = nextPosition * _speed;
        }
    }
}