using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class CameraFollow : NetworkBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothTime = 0.1f;

        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;

        private Character _target;
        private Vector3 _currentVelocity = Vector3.zero;

        private bool _isFirstFrame = true;

        public void Initialize(Character target)
        {
            _target = target;
            _isFirstFrame = true;
        }

        public override void Render()
        {
            if (_target == null)
                return;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector3 targetPosition = _target.transform.position;
            Vector3 nextPosition = targetPosition + _offset;

            if (_isFirstFrame)
            {
                _isFirstFrame = false;
                transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);
                return;
            }
            
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, nextPosition, ref _currentVelocity, _smoothTime);

            float clampedX = Mathf.Clamp(smoothPosition.x, _minBounds.x, _maxBounds.x);
            float clampedY = Mathf.Clamp(smoothPosition.y, _minBounds.y, _maxBounds.y);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}