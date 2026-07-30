using TMPro;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public class PositionPlayersView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;

        public void UpdateUI(Vector2 position)
        {
            if (position != null)
                _position.text = $"(X: {position.x:F2}, Y: {position.y:F2})";
        }
    }
}