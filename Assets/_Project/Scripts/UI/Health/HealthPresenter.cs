using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.UI.Health
{
    public class HealthPresenter : NetworkBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private WorldHealthBar _worldHealth;

        public override void Spawned()
        {
            if (_healthBarView != null)
                _healthBarView.gameObject.SetActive(HasInputAuthority);

            if (_worldHealth != null)
                _worldHealth.gameObject.SetActive(!HasInputAuthority);
        }

        public override void Render()
        {
            if (HasInputAuthority)
                _healthBarView?.UpdateHealthBar(_character.CurrentHealth, _character.MaxHealth);
            else
                _worldHealth?.UpdateHealthBar(_character.CurrentHealth, _character.MaxHealth);
        }
    }
}