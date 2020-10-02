using Onward.Game.Enums;
using UnityEngine;
using Zenject;

namespace Onward.Game.MonoBehaviours
{
    public class GameManager : MonoBehaviour
    {
        public bool isStateCombat;

        public void ToggleGameState()
        {
            isStateCombat = !isStateCombat;
        }
        
    }
}