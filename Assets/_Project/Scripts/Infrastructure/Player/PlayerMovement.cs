using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NetworkRigidbody2D))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float _baseMoveSpeed = 4f;

        [Header("Player Bounds")]
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;
        
        private Rigidbody2D _head;
        private Vector2 _moveDirection;

        private bool _isMovingLeft;

        [Networked] public float MoveSpeed { get; set; }

        public Vector2 Position => transform.position;

        public override void Spawned()
        {
            _head = GetComponent<Rigidbody2D>();
            _head.bodyType = RigidbodyType2D.Kinematic;

            MoveSpeed = _baseMoveSpeed;
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

        public override void FixedUpdateNetwork()
        {
            Move();
        }

        private void Move()
        {
            if (GetInput(out InputController  controller))
            {
                _moveDirection = new Vector2(controller.HorizontalInput, controller.VerticalInput);
                _head.velocity = _moveDirection * MoveSpeed;
                
                Vector2 headPosition = _head.position;

                float clampedX = Mathf.Clamp(headPosition.x, _minBounds.x, _maxBounds.x);
                float clampedY = Mathf.Clamp(headPosition.y, _minBounds.y, _maxBounds.y);

                Vector2 finalPosition = new Vector2(clampedX, clampedY);

                _head.position = finalPosition;
            }
            else
            {
                _head.velocity = Vector2.zero;
            }
        }
    }
}