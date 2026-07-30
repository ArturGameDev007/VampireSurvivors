using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.ExperiencePlayer
{
    public class ExperienceView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        
        private float _minValue = 0f;

        private void Start()
        {
            _slider.value = _minValue;
        }

        public void UpdateExperienceBar(float currentValue, float maxValue)
        {
            if (_slider != null)
            {
                _slider.maxValue = maxValue;
                _slider.value = currentValue;
            }
        }
    }
}