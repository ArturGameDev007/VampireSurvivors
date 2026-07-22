// using System;
// using _Project.Scripts.Infrastructure.EnemyInformation;
// using _Project.Scripts.UI.Health;
// using Fusion;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
//
// namespace _Project.Scripts.Infrastructure.Player
// {
//     public class PlayerHealth : NetworkBehaviour
//     {
//         [SerializeField] private float _maxHealth = 100f;
//
//         private HealthBarView _healthBarView;
//
//         [Networked] public float CurrentHealth { get; private set; }
//
//         public void Initialize(HealthBarView healthBarView)
//         {
//             _healthBarView = healthBarView;
//         }
//
//         public override void Spawned()
//         {
//             if (Runner.IsServer)
//             {
//                 CurrentHealth = _maxHealth;
//             }
//         }
//
//         public override void Render()
//         {
//             _healthBarView?.UpdateHealthBar(CurrentHealth);
//         }
//
//         public void TakeDamage(float amount)
//         {
//             CurrentHealth -= amount;
//             CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, _maxHealth);
//
//             if (CurrentHealth <= 0f)
//             {
//                 Die();
//             }
//         }
//
//         private void Die()
//         {
//             Debug.Log("Игрок погиб. Перезапуск");
//             
//         }
//
//         private void OnTriggerEnter2D(Collider2D other)
//         {
//             // if (Runner == null || !Runner.IsServer)
//             //     return;
//
//             if (!HasInputAuthority)
//                 return;
//             
//             if (other.TryGetComponent(out Enemy enemy))
//             {
//                 TakeDamage(enemy.Damage);
//             }
//         }
//     }
// }