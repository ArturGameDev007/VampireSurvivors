using _Project.Scripts.Infrastructure.EnemyInformation;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.Player
{
    public class Character : NetworkBehaviour
    {
        [SerializeField] private PlayerBonus _playerBonus;
        
        [SerializeField] private float _baseMaxHealth = 100f;
        
        [field: SerializeField] public float MaxExperience { get; private set; } = 100f;

        private Camera _mainCamera;
        private PlayerRegistry _playerRegistry;
        private PlayerMovement _playerMovement;
        private BonusApplier _bonusApplier;
        
        [Networked] public bool IsDead { get; private set; }

        [Networked] public float CurrentExperience { get; private set; }
        [Networked] public float CurrentHealth { get; private set; }
        [Networked] public float MaxHealth { get; private set; }
        [Networked] public int CurrentLevel { get; private set; }

        public void Initialize(PlayerRegistry playerRegistry, BonusApplier bonusApplier)
        {
            _playerRegistry = playerRegistry;
            _bonusApplier = bonusApplier;
        }

        public override void Spawned()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            if (Runner.IsServer)
            {
                MaxHealth = _baseMaxHealth;
                CurrentHealth = MaxHealth;
                CurrentLevel = 1;
            }

            gameObject.name = HasInputAuthority ? "[LOCAL_PLAYER]" : $"[REMOTE_PLAYER]";

            if (!HasInputAuthority)
                return;

            _mainCamera = Camera.main;

            if (_mainCamera != null && _mainCamera.TryGetComponent(out CameraFollow cameraFollow))
                cameraFollow.Initialize(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            NotifyDeath();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!HasStateAuthority)
                return;

            if (other.TryGetComponent(out Enemy enemy))
                TakeDamage(enemy.Damage);
        }

        public void Heal(float amount)
        {
            if (!HasStateAuthority)
                return;

            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, MaxHealth);
        }

        public void Experience(float amount)
        {
            if (!HasStateAuthority)
                return;

            CurrentExperience = Mathf.Clamp(CurrentExperience + amount, 0f, MaxExperience);
        }

        public void NextLevel(int level)
        {
            if (!HasStateAuthority)
                return;

            CurrentLevel = level;
            CurrentHealth = MaxHealth;
            CurrentExperience = 0f;
            
            _bonusApplier.ApplyRandomBonus(this, _playerBonus, _playerMovement);
        }

        public void IncreaseMaxHealth(float percent)
        {
            if (!HasStateAuthority)
                return;

            MaxHealth += MaxHealth * percent;
            CurrentHealth = MaxHealth;
        }
        
        private void NotifyDeath()
        {
            if (HasInputAuthority)
            {
                ReturnToLobby();
            }
        }

        private void Die()
        {
            if (!HasStateAuthority)
                return;

            IsDead = true;

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

        private void ReturnToLobby()
        {
            if (Runner != null)
            {
                PlayerPrefs.SetInt($"BannedRoom - {Runner.SessionInfo.Name}", 1);
                PlayerPrefs.Save();
                
                Runner.Shutdown(destroyGameObject: false);
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}