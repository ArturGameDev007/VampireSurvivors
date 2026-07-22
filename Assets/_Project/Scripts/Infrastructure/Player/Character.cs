using _Project.Scripts.Infrastructure.EnemyInformation;
using _Project.Scripts.UI.Health;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.Player
{
    public class Character : NetworkBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private WorldHealthBar _worldHealth;
        [SerializeField] private PositionPlayers _positionPlayers;

        [field: SerializeField] public float MaxHealth { get; private set; } = 100f;

        private Camera _mainCamera;
        private PlayerRegistry _playerRegistry;
        private PlayerMovement _playerMovement;

        private bool _isDead;

        [Networked] public float CurrentHealth { get; private set; }

        public void Initialize(PlayerRegistry playerRegistry)
        {
            _playerRegistry = playerRegistry;
        }

        public override void Spawned()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            if (Runner.IsServer)
                CurrentHealth = MaxHealth;

            if (_healthBarView != null)
                _healthBarView.gameObject.SetActive(HasInputAuthority);

            if (_worldHealth != null)
                _worldHealth.gameObject.SetActive(!HasInputAuthority);
            
            if (!HasInputAuthority)
                return;

            _mainCamera = Camera.main;

            if (_mainCamera != null && _mainCamera.TryGetComponent(out CameraFollow cameraFollow))
            {
                cameraFollow.Initialize(this);
            }
        }

        public override void Render()
        {
            if (HasInputAuthority)
            {
                _healthBarView?.UpdateHealthBar(CurrentHealth, MaxHealth);
            }
            else
            {
                _worldHealth?.UpdateHealthBar(CurrentHealth, MaxHealth);
                UpdateCoordinateUI();
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (_isDead && HasInputAuthority)
            {
                ReturnToLobby();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!HasStateAuthority)
                return;

            if (other.TryGetComponent(out Enemy enemy))
                TakeDamage(enemy.Damage);
        }

        private void Die()
        {
            _isDead = true;

            _playerRegistry?.Unregister(this);

            Runner.Despawn(Object);
        }

        private void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            if (CurrentHealth <= 0f)
                Die();
        }

        private void UpdateCoordinateUI()
        {
            if (_playerMovement == null || _playerMovement.Position == null)
                return;

            Vector2 direction = _playerMovement.Position;
            string displayText = $"(X: {direction.x:F2}, Y: {direction.y:F2})";

            _positionPlayers.SetCoordinateText(displayText);
        }

        private void ReturnToLobby()
        {
            if (Runner != null)
                Runner.Shutdown(destroyGameObject: true);

            SceneManager.LoadScene("MainMenu");
        }
    }
}