using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NetworkRigidbody2D))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed = 4f;

        [Header("Player Bounds")]
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;

        private Rigidbody2D _head;
        private Vector2 _moveDirection;

        private bool _isMovingLeft;
        
        public Vector2 Position => transform.position;

        public override void Spawned()
        {
            _head = GetComponent<Rigidbody2D>();
            _head.bodyType = RigidbodyType2D.Kinematic;
        }

        public override void FixedUpdateNetwork()
        {
            Move();
        }

        private void Move()
        {
            if (GetInput(out InputController input))
            {
                _moveDirection = new Vector2(input.HorizontalInput, input.VerticalInput).normalized;
                
                _head.velocity = _moveDirection * _moveSpeed;
                
                Vector2 headPosition = _head.position;
                
                float clampedX = Mathf.Clamp(headPosition.x, _minBounds.x, _maxBounds.x);
                float clampedY = Mathf.Clamp(headPosition.y, _minBounds.y, _maxBounds.y);
                
                Vector2 finalPosition = new Vector2(clampedX, clampedY);
                
                _head.position = finalPosition;
                
                // ClampPosition();
            }
            else
            {
                _head.velocity = Vector2.zero;
            }
        }

        public override void Render()
        {
            if (_moveDirection == Vector2.zero)
                return;

            Vector2 currentScale = transform.localScale;

            if (Mathf.Sign(_moveDirection.x) != Mathf.Sign(currentScale.x))
            {
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }
        }

        private void ClampPosition()
        {
            Vector3 currentPosition = transform.position;

            float clampedX = Mathf.Clamp(currentPosition.x, _minBounds.x, _maxBounds.x);
            float clampedY = Mathf.Clamp(currentPosition.y, _minBounds.y, _maxBounds.y);

            transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        }
    }
}