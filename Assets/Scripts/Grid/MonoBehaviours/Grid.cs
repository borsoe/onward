using Onward.Grid.Classes;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.Grid.MonoBehaviours
{
    /// <summary>
    /// this holds a data representation of the whole map
    /// </summary>
    public class Grid: MonoBehaviour
    {
        private Node[,] _nodes;

        // [Inject(Id = "enemy")]
        // private Tilemap _enemyGround;
        //
        // [Inject(Id = "ally")]
        // private Tilemap _allyGround;
        
        public Node this[int i, int j]
        {
            get => _nodes[i, j];
            set => _nodes[i, j] = value;
        }
    }
}