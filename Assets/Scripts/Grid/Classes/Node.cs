

using System.Collections.Generic;
using Onward.Character.MonoBehaviours;
using UnityEngine;
using Zenject.ReflectionBaking.Mono.Cecil.Cil;

namespace Onward.Grid.Classes
{
    /// <summary>
    /// the data representation of a single node
    /// </summary>
    public class Node
    {
        private Entity _occupyingEntity;
        
        
        public Vector3 Location;
        public Vector3Int LocationInGrid;
        public List<Node> AdjacentNodes;
        public bool IsNodeAlly;

        /// <summary>
        /// to be used by the BFS algorithm
        /// </summary>
        public Node PrevNode;

        /// <summary>
        /// to be selected by the BFS algorithm
        /// </summary>
        public Node NextNode;

        public Entity OccupyingEntity
        {
            get => _occupyingEntity;
            set
            {
                _occupyingEntity = value;
                if (_occupyingEntity != null)
                {
                    _occupyingEntity.GoToLocation(Location);
                }
            }
        }
    }
}