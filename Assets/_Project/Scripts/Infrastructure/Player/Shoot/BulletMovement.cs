using _Project.Scripts.Infrastructure.EnemyInformation;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player.Shoot
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NetworkRigidbody2D))]
    public class BulletMovement : NetworkBehaviour
    {
        [SerializeField] private float _speed = 15f;

        [Networked] public Enemy TargetEnemy { get; private set; }

        private Rigidbody2D _rigidbody2D;

        public override void Spawned()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        public void Initialize(Enemy target)
        {
            TargetEnemy = target;
        }

        public override void FixedUpdateNetwork()
        {
            Move();
        }

        private void Move()
        {
            if (TargetEnemy == null)
                return;

            Vector2 currentPosition = _rigidbody2D.position;
            Vector2 target = TargetEnemy.transform.position;
            Vector2 directionToEnemy = Vector2.MoveTowards(currentPosition, target, _speed * Runner.DeltaTime);

            _rigidbody2D.MovePosition(directionToEnemy);
        }
    }
}