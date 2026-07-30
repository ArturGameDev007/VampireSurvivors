using _Project.Scripts.Infrastructure.Player.Shoot;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public class BonusApplier
    {
        private const string AttackSpeedBonus = "IncreaseAttackSpeed";
        private const string HealthBarBonus = "IncreaseHealthBar";
        private const string MoveSpeedBonus = "IncreaseMoveSpeed";

        public void ApplyRandomBonus(Character character, PlayerBonus playerBonus, PlayerMovement playerMovement)
        {
            string[] allBonuses = { AttackSpeedBonus, HealthBarBonus, MoveSpeedBonus };
            int minvalue = 0;

            var bonus = allBonuses[Random.Range(minvalue, allBonuses.Length)];

            Debug.Log($"Применен бонус - {bonus}");

            switch (bonus)
            {
                case AttackSpeedBonus:
                    if (character.TryGetComponent(out ShootController shootController))
                    {
                        shootController.IncreaseAttackSpeed(playerBonus.IncreaseAttackSpeed);
                        Debug.Log($"Текущая задержка атаки - {shootController.CurrentDelayAttack}");
                    }

                    break;

                case HealthBarBonus:
                    character.IncreaseMaxHealth(playerBonus.IncreaseHealthBar);
                    break;

                case MoveSpeedBonus:
                    playerMovement.MoveSpeed += playerMovement.MoveSpeed * playerBonus.IncreaseMoveSpeed;
                    break;
            }
        }
    }
}