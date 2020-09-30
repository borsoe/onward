using System.Collections.Generic;
using Onward.AI.Interfaces;
using Onward.Character.MonoBehaviours;
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

        public List<IAiListener> Listeners
        {
            set => _nextListeners = value;
        }

        [Inject]
        public AiManager(List<IAiListener> currentListeners)
        {
            _nextListeners = currentListeners;
            _currentListeners = new List<IAiListener>();
        }

        public void Tick()
        {
            for (int i = 0; i < _currentListeners.Count; i++)
            {
                if (_nextListeners != null) _currentListeners.AddRange(_nextListeners);
                _nextListeners = null;
                var listener = _currentListeners[i];
                if (listener.CheckTarget())
                {
                    listener.Attack();
                    _currentListeners.Remove(listener);
                    i--;
                }else if (listener.FindPathToNearestTarget())
                {
                    listener.MoveToward();
                    _currentListeners.Remove(listener);
                    i--;
                }
            }
        }
    }
}