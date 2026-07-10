using _Project.Scripts.Infrastructure.Player;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    public class EnemySpawnController
    {
        public readonly IGameFactory _gameFactory;
        public readonly GenerateEnemies _generateEnemies;

        public EnemySpawnController(IGameFactory gameFactory, GenerateEnemies generateEnemies)
        {
            _gameFactory = gameFactory;
            _generateEnemies = generateEnemies;
        }

        public void Enable()
        {
            _gameFactory.OnCharacterCreated += OnPlayerSpawned;
        }

        public void Disable()
        {
            if (_gameFactory != null)
                _gameFactory.OnCharacterCreated -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(Character character)
        {
            _generateEnemies.SetPlayerProvider(character);
        }
    }
}