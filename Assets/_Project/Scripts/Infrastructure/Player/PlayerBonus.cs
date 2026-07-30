using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    [Serializable]
    public class PlayerBonus
    {
        [field: SerializeField] public float IncreaseAttackSpeed { get; private set; } = 0.10f;
        [field: SerializeField] public float IncreaseHealthBar { get; private set; } = 0.10f;
        [field: SerializeField] public float IncreaseMoveSpeed { get; private set; } = 0.05f;
    }
}