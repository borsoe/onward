using System.Collections.Generic;
using Onward.AI.Interfaces;
using Onward.Character.MonoBehaviours;
using Onward.Game.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.AI.Classes
{
    /// <summary>
    /// the singleton-ish class that is responsible for general ai behaviours
    /// </summary>
    public class AiManager : ITickable
    {
        private List<IAiListener> _currentListeners;

        private List<IAiListener> _nextListeners;

        private GameManager _gameManager;
        
        // public IAiListener NewListener
        // {
        //     set => _nextListeners.Add(value);
        // }

        public void SetNewListener(IAiListener listener)
        {
            _nextListeners.Add(listener);
        }

        [Inject]
        public AiManager(List<IAiListener> currentListeners, GameManager gameManager)
        {
            _currentListeners = currentListeners;
            _gameManager = gameManager;
            _nextListeners = new List<IAiListener>();
            
        }

        public void Tick()
        {
            // Debug.Log($"current listeners is: {_currentListeners.Count} and next is: {_nextListeners.Count}");
            if (!_gameManager.isStateCombat) return;
            // get all the listeners that are scheduled 
            if (_nextListeners.Count != 0)
            {
                _currentListeners.AddRange(_nextListeners);
                _nextListeners.Clear();
            }
            for (int i = 0; i < _currentListeners.Count; i++)
            {
                var listener = _currentListeners[i];
                // if there is an available target, attack it!
                if (listener.CheckTarget())
                {
                    listener.Attack();
                    _currentListeners.Remove(listener);
                    i--;
                }else if (listener.FindPathToNearestTarget()) // search for new target
                {
                    // if the new target is in range, attack it
                    if (listener.CheckTarget())
                        listener.Attack();
                    else // if not, move toward it
                        listener.MoveToward();
                    _currentListeners.Remove(listener);
                    i--;
                }
            }
        }
    }
}