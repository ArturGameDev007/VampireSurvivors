using _Project.Scripts.Infrastructure.Player;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.UI.CharacterMovementController
{
    public class CoordinatePresenter : NetworkBehaviour
    {
        [SerializeField] private PositionPlayersView  _positionPlayersView;

        private PlayerMovement _playerMovement;

        public override void Spawned()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public override void Render()
        {
            if (!HasInputAuthority)
            {
                _positionPlayersView?.UpdateUI(_playerMovement.Position);
            }
        }
    }
}