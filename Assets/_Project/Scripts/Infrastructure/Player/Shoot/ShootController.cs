using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player.Shoot
{
    public class ShootController : NetworkBehaviour
    {
        [SerializeField] private Transform _pointBullet;

        private GenerateBullets _generateBullets;
        public float CurrentDelayAttack => _generateBullets.CurrentDelay;

        public void Initialize(GenerateBullets generateBullets)
        {
            _generateBullets = generateBullets;
        }

        public override void FixedUpdateNetwork()
        {
            if (_generateBullets == null)
                return;

            _generateBullets.Process(_pointBullet);
        }

        public void IncreaseAttackSpeed(float percent)
        {
            _generateBullets?.IncreaseSpeedShoot(percent);
        }
    }
}