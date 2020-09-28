using Onward.Character.ScriptableObjects;
using UnityEngine;

namespace Onward.Character.MonoBehaviours
{
    public class CharacterEntity : MonoBehaviour
    {
        public CharacterData characterData;
        
        public void GoToLocation(Vector3 pos)
        {
            //TODO add animation if necessary
            transform.position = pos;
        }
    }
}