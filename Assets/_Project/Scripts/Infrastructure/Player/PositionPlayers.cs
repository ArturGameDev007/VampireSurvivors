using TMPro;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public class PositionPlayers : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;

        public void SetCoordinateText(string text)
        {
            _position.text = text;
        }
    }
}