using System;
using UnityEngine;
using UnityEngine.UI;

namespace VRUEAssignments.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillSprite;
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        
        [SerializeField] private bool _animate = true;
        [SerializeField] private float _reduceSpeed = 0.75f;
        private float _target = 0f;

        
        public void UpdateHealthBar(float maxHealth, float currentHealth)
        {
            _target = currentHealth / maxHealth;
            _fillSprite.color = _gradient.Evaluate(_target);
        }

        public void Update()
        {
            if (!_animate)
            {
                _slider.value = _target;
            }
            else
            {
                _slider.value =
                    Mathf.MoveTowards(_slider.value, _target, _reduceSpeed * Time.deltaTime);
            }
        }
    }
}