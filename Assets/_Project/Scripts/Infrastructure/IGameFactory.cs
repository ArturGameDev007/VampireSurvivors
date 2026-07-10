using System;
using _Project.Scripts.Infrastructure.Player;
using Fusion;

namespace _Project.Scripts.Infrastructure
{
    public interface IGameFactory
    {
        public event Action<Character> OnCharacterCreated;

        public void CreatePlayer(NetworkRunner runner, Character prefab, PlayerRef player);
    }
}