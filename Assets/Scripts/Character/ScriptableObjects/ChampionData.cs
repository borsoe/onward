using UnityEngine;

namespace Onward.Character.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Onward/Data/CharacterData", order = 0)]
    public class ChampionData : ScriptableObject
    {
        public int baseAttackRange;
        public int attackRange;
        public float baseAttackDamage;
        public float attackDamage;
        public float baseHealth;
        private float _health;

        public delegate void HealthChangeDelegate(float value);
        public HealthChangeDelegate onHealthChange;
        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                //TODO delegate
                onHealthChange(_health);
            }
        }
        /// <summary>
        /// How many nodes can this character travers in a second, times 100?
        /// </summary>
        public float baseMoveSpeed; 
        public float moveSpeed;
        /// <summary>
        /// How many attacks can this character do per seconds?
        /// </summary>
        public float baseAttackSpeed;
        public float attackSpeed;
    }
}