using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.EnemyInformation
{
    public class EnemyRegistry
    {
        private readonly List<Enemy> _activeEnemies = new();
        
        public List<Enemy>  ActiveEnemies => _activeEnemies;
        
        public void Add(Enemy enemy)
        {
            if (enemy == null)
                return;

            if (!_activeEnemies.Contains(enemy))
                _activeEnemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
            if (enemy == null)
                return;

            if (_activeEnemies.Contains(enemy))
                _activeEnemies.Remove(enemy);
        }
    }
}