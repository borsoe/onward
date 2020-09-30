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
        public float health;
    }
}