using System.Collections.Generic;
using Onward.Character.ScriptableObjects;
using Onward.IOC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Onward.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EncounterDesign", menuName = "Onward/EncounterDesign", order = 1)]
    public class EncounterDesign : SerializedScriptableObject
    {
        public Dictionary<Vector3, ChampionData> enemies;

        public GameObject temp;
        
        [Button(ButtonSizes.Medium)]
        private void GetEnemiesByReference()
        {
            enemies = new Dictionary<Vector3, ChampionData>();
            foreach (Transform child in temp.transform)
            {
                enemies.Add(child.position, child.GetComponent<CharacterInstaller>().championData);
            }
        }
    }
}