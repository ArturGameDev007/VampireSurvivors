using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Health
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (_slider != null)
            {
                _slider.maxValue = maxHealth;
                _slider.value = currentHealth;
            }
            else
            {
                Debug.LogWarning("Slider is null!");
            }
        }
    }
}