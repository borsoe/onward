using System.Collections.Generic;
using Onward.AI.Interfaces;
using Onward.Character.Interfaces;
using Onward.Character.MonoBehaviours;
using Onward.Grid.Classes;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;
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
        private List<Entity> _initialCharacters;
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
            List<Entity> initialCharacters, UnityEngine.Grid grid)
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
                    OccupyingEntity = null,
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
                this[position].OccupyingEntity = character;
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

        public int Distance(Vector3 to, Vector3 from)
        {
            var t = _grid.WorldToCell(to);
            var f = _grid.WorldToCell(from);
            return Mathf.Max(Mathf.Abs(t.x - f.x), Mathf.Abs(t.y - f.y), Mathf.Abs(t.y - f.y));
        }

        public bool BFS(Vector3 origin, int range, out IAttackAble target)
        {
            var visitedNodes = new HashSet<Node>();
            var toBeVisited = new HashSet<Node>();
            var queue = new Queue<Node>();
            var originalEntity = this[origin].OccupyingEntity;
            queue.Enqueue(this[origin]);
            target = null;
            Node vertex = null;
            while (queue.Count > 0)
            {
                vertex = queue.Dequeue();
                
                //check if I can attack from here
                if (_RangeBfs(range, ref target, vertex, visitedNodes, originalEntity))
                {
                    _BuildPath(origin, vertex);
                    return true;
                }
                if (!visitedNodes.Add(vertex))
                    continue;

                for (int i = 0; i < vertex.AdjacentNodes.Count; i++)
                {
                    var neighbour = vertex.AdjacentNodes[i];
                    
                    if (neighbour.OccupyingEntity != null && neighbour.OccupyingEntity.faction != originalEntity.faction)
                    {
                        target = (IAttackAble) neighbour.OccupyingEntity;
                        _BuildPath(origin, vertex);
                        return true;
                    }    
                    if (visitedNodes.Contains(neighbour) ||
                        _IsAlly(neighbour, originalEntity)) continue;
                    if (!toBeVisited.Add(neighbour)) continue;
                    queue.Enqueue(neighbour);
                    neighbour.PrevNode = vertex;
                }
            }
            return false;
        }

        private static bool _IsAlly(Node neighbour, Entity originalEntity)
        {
            return neighbour.OccupyingEntity != null && neighbour.OccupyingEntity.faction == originalEntity.faction;
        }

        private void _BuildPath(Vector3 origin, Node vertex)
        {
            var tmp = vertex;
            var last = this[origin];
            if (tmp == last)
            {
                tmp.NextNode = tmp;
                return;
            }
            while (tmp != last)
            {
                tmp.PrevNode.NextNode = tmp;
                tmp = tmp.PrevNode;
            }
        }

        private static bool _RangeBfs(int range, ref IAttackAble target, Node vertex, HashSet<Node> visitedNodes,
            Entity originalEntity)
        {
            var rangeQueue = new Queue<Node>();
            var rangeVisited = new HashSet<Node>();
            var toBeRangeVisited = new HashSet<Node>();
            rangeQueue.Enqueue(vertex);
            int depth = 0;
            int stepsToIncreaseDepth = 1;
            while (rangeQueue.Count > 0)
            {
                var v = rangeQueue.Dequeue();
                stepsToIncreaseDepth--;
                if (!rangeVisited.Add(v) || visitedNodes.Contains(v))
                    continue;
                target = v.OccupyingEntity as IAttackAble;
                if (target != null && target.CanBeAttackedBy(originalEntity))
                    return true;
                for (int i = 0; i < v.AdjacentNodes.Count; i++)
                {
                    var n = v.AdjacentNodes[i];
                    if (visitedNodes.Contains(n) || rangeVisited.Contains(n)) continue;
                    if (toBeRangeVisited.Add(n))
                    {
                        rangeQueue.Enqueue(n);
                    }
                }
                if (stepsToIncreaseDepth == 0)
                {
                    depth++;
                    stepsToIncreaseDepth = rangeQueue.Count;
                }
                if (depth > range)
                    return false;
            }
            return false;
        }
    }
}