using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.ExperiencePlayer
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public void SetLevel(int level)
        {
            _levelText.text = $"Level {level}";
        }
    }
}