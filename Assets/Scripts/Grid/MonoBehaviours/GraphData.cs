using System;
using System.Collections.Generic;
using System.Linq;
using Onward.Character.MonoBehaviours;
using Onward.Grid.Classes;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Zenject;

namespace Onward.Grid.MonoBehaviours
{
    /// <summary>
    /// this holds a data representation of the whole map
    /// </summary>
    public class GraphData: MonoBehaviour
    {
        private Dictionary<Vector3, Node> _nodes;
        private Tilemap _enemyGround;
        private Tilemap _allyGround;
        private List<CharacterEntity> _initialCharacters;
        private UnityEngine.Grid _grid;
        
        public Node this[Vector3 pos]
        {
            set => _nodes[pos] = value;
            get
            {
                if(_nodes.TryGetValue(pos, out var node)) return node;
                var fixedPos = _allyGround.HasTile(_allyGround.WorldToCell(pos))
                    ? _allyGround.GetCellCenterWorld(_allyGround.WorldToCell(pos))
                    : _enemyGround.GetCellCenterWorld(_enemyGround.WorldToCell(pos));
                if(_nodes.TryGetValue(fixedPos, out var fixedNode)) return fixedNode;
                Debug.LogError($"position {pos} dose not exist in the graph");
                return null;
            }
        }
        
        [Inject]
        public void Construct([Inject(Id = "enemy")] Tilemap enemyGrounds, [Inject(Id = "ally")] Tilemap allyGrounds, 
            List<CharacterEntity> initialCharacters, UnityEngine.Grid grid)
        {
            _enemyGround = enemyGrounds;
            _allyGround = allyGrounds;
            _initialCharacters = initialCharacters;
            _grid = grid;
        }

        private void Awake()
        {
            _Init();
        }

        /// <summary>
        /// initialize the data graph. call this in the awake
        /// </summary>
        private void _Init()
        {
            _nodes = new Dictionary<Vector3, Node>();
            _HandleGround(_allyGround, true);
            _HandleGround(_enemyGround, false);
            _HandleCharacters();
            _HandleAdjacency();
        }

        private void _HandleGround(Tilemap ground, bool isAlly)
        {
            foreach (var pos in ground.cellBounds.allPositionsWithin)
            {
                if (!ground.HasTile(pos)) continue;
                var cellCenterWorldLocation = ground.GetCellCenterWorld(pos);
                this[cellCenterWorldLocation] = new Node
                {
                    Location = cellCenterWorldLocation,
                    LocationInGrid = pos,
                    OccupyingCharacterEntity = null,
                    IsNodeAlly = isAlly,
                    AdjacentNodes = new List<Node>()
                };
            }
        }

        private void _HandleCharacters()
        {
            foreach (var character in _initialCharacters)
            {
                var transform1 = character.transform;
                var position = transform1.position;
                position = this[position].Location;
                transform1.position = position;
                this[position].OccupyingCharacterEntity = character;
            }
        }


        private void _HandleAdjacency()
        {
            var cellSize = _grid.cellSize;
            foreach (var node in _nodes.Values)
            {
                var values = _GetAllAdjacentCells(node, cellSize);
                foreach (var pos in values)
                {
                    if (_allyGround.HasTile(_allyGround.WorldToCell(pos)) || 
                        _enemyGround.HasTile(_enemyGround.WorldToCell(pos)))
                    {
                        node.AdjacentNodes.Add(this[pos]);
                    }
                }
            }
        }

        private static IEnumerable<Vector3> _GetAllAdjacentCells(Node node, Vector3 cellSize)
        {
            var values = new List<Vector3>
            {
                new Vector3(node.Location.x - cellSize.x, node.Location.y, 0f),
                new Vector3(node.Location.x - cellSize.x / 2, node.Location.y + cellSize.y, 0f),
                new Vector3(node.Location.x + cellSize.x / 2, node.Location.y + cellSize.y, 0f),
                new Vector3(node.Location.x + cellSize.x, node.Location.y, 0f),
                new Vector3(node.Location.x - cellSize.x / 2, node.Location.y - cellSize.y, 0f),
                new Vector3(node.Location.x + cellSize.x / 2, node.Location.y - cellSize.y, 0f)
            };
            return values;
        }
    }
}