

using System.Collections.Generic;
using Onward.Character.MonoBehaviours;
using UnityEngine;

namespace Onward.Grid.Classes
{
    /// <summary>
    /// the data representation of a single node
    /// </summary>
    public class Node
    {
        private CharacterEntity _occupyingCharacterEntity;
        
        
        public Vector3 Location;
        public Vector3Int LocationInGrid;
        public List<Node> AdjacentNodes;
        public bool IsNodeAlly;

        public CharacterEntity OccupyingCharacterEntity
        {
            get => _occupyingCharacterEntity;
            set
            {
                _occupyingCharacterEntity = value;
                if (_occupyingCharacterEntity != null)
                {
                    _occupyingCharacterEntity.GoToLocation(Location);
                }
            }
        }
    }
}