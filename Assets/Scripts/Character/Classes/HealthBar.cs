using UnityEngine;

namespace Onward.Character.Classes
{
    public class HealthBar
    {
        private Gradient _gradient;
        private float _currentHealth;
        private Transform _bar;
        private SpriteRenderer _barSprite;

        public HealthBar(Transform bar, Gradient gradient, SpriteRenderer barrSprite)
        {
            _bar = bar;
            _gradient = gradient;
            _barSprite = barrSprite;
        }

        public void UpdateHealth(float value)
        {
            var scale = _bar.localScale;
            scale.x = 1 / value;
            _bar.localScale = scale;
            _barSprite.color = _gradient.Evaluate(scale.x);
        }
    }
}