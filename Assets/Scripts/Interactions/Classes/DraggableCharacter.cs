using Onward.Interactions.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.Interactions.Classes
{
    public class DraggableCharacter : MonoBehaviour, IDraggable
    {

        private Tilemap _allowedTiles;

        private Vector3 _initialPos;

        [Inject]
        public void Construct(Tilemap allowedTiles)
        {
            _allowedTiles = allowedTiles;
        }
        
        public void OnDragBegin(Vector3 pos)
        {
            _initialPos = transform.position;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position = pos;
        }

        public void OnDrag(Vector3 pos)
        {
            transform.position = pos;
        }

        public void OnDragEnd(Vector3 pos)
        {
            var cellPos = _allowedTiles.WorldToCell(pos);
            transform.position = _allowedTiles.GetCellCenterWorld(cellPos);
        }
    }
}