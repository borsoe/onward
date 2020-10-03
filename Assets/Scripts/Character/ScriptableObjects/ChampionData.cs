﻿using Onward.Character.Classes;
using UnityEngine;

namespace Onward.Character.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Onward/Data/CharacterData", order = 0)]
    public class ChampionData : ScriptableObject
    {
        /// <summary>
        /// how many hexagon away can this character attack?
        /// </summary>
        [Header("base attributes")]
        public int attackRange;
        /// <summary>
        /// how much base physical damage will this character deal per attack?
        /// </summary>
        public float attackDamage;
        /// <summary>
        /// what is this character max health?
        /// </summary>
        public float maxHealth;
        /// <summary>
        /// How many nodes can this character travers in a second, times 100?
        /// </summary> 
        public float moveSpeed;
        /// <summary>
        /// How many attacks can this character do per seconds?
        /// </summary>
        public float attackSpeed;

        
        [Header("range attack")] 
        public Sprite attackProjectileSprite;
        public float rangeAttackTravelSpeed;
    }
}