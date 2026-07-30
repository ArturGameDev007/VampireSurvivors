using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public class PlayerRegistry
    {
        private readonly List<Character> _players = new();
        
        public List<Character> AddPlayers => _players;

        public void Register(Character player)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);
            }
        }

        public void Unregister(Character player)
        {
            _players.Remove(player);
        }

        public Character GetClosestTo(Vector3 position)
        {
            Character closest = null;
            float minDist = float.MaxValue;

            foreach (var p in _players)
            {
                if (p == null) continue;

                float dist = Vector3.Distance(position, p.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = p;
                }
            }

            return closest;
        }
    }
}