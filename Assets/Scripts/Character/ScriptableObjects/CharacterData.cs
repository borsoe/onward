using UnityEngine;

namespace Onward.Character.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Character Data", menuName = "Onward/Data/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string name;
    }
}