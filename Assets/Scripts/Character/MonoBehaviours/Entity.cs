using System;
using Onward.AI.Interfaces;
using Onward.Character.Interfaces;
using Onward.Character.ScriptableObjects;
using Onward.Grid.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.Character.MonoBehaviours
{
    /// <summary>
    /// Base class for anything on board
    /// </summary>
    public class Entity : MonoBehaviour
    {
        public virtual void GoToLocation(Vector3 pos)
        {
            transform.position = pos;
        }
        public Faction faction;
    }
}