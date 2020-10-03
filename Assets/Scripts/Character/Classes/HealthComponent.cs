using Onward.Character.ScriptableObjects;
using Zenject;

namespace Onward.Character.Classes
{
    /// <summary>
    /// the component that is responsible for any entities health
    /// </summary>
    public class HealthComponent
    {
        #region setup

        private float _health;
        private float _maxHealth;
        
        private delegate void HealthChangeDelegate(float value);
        private readonly HealthChangeDelegate _onHealthChange;
        
        [Inject]
        public HealthComponent(HealthBar healthBar, ChampionData championData)
        {
            _health = championData.maxHealth;
            _maxHealth = championData.maxHealth;
            _onHealthChange = healthBar.UpdateHealth;
        }
        #endregion
        
        #region properties
        
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

        #endregion
    }
}