using Zenject;

namespace Onward.Character.Classes
{
    /// <summary>
    /// the component that is responsible for any entities health
    /// </summary>
    public class HealthComponent
    {
        private float _health;
        private float _maxHealth;
        
        private delegate void HealthChangeDelegate(float value);
        private readonly HealthChangeDelegate _onHealthChange;

        public float Health
        {
            get => _health;
            set
            {
                _onHealthChange(_health / _maxHealth);
                _health = value;
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        [Inject]
        public HealthComponent(HealthBar healthBar, [Inject]float health)
        {
            _health = health;
            _maxHealth = health;
            _onHealthChange = healthBar.UpdateHealth;
        }
    }
}