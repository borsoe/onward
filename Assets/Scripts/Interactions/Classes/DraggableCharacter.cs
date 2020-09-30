using Onward.Character.MonoBehaviours;
using Onward.Grid.MonoBehaviours;
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
        private GraphData _graphData;
        private Entity _entity;
        
        [Inject]
        public void Construct([Inject(Id = "ally")]Tilemap allowedTiles, GraphData graphData,
            Entity entity)
        {
            _allowedTiles = allowedTiles;
            _graphData = graphData;
            _entity = entity;
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
            var targetPos =
                _allowedTiles.HasTile(cellPos) ? _allowedTiles.GetCellCenterWorld(cellPos) : _initialPos;
            if (_graphData[targetPos].OccupyingEntity == null)
            {
                _graphData[targetPos].OccupyingEntity = _entity;
                _graphData[_initialPos].OccupyingEntity = null;
                // transform.position = targetPos;
            }else if (_graphData[targetPos].OccupyingEntity == _entity)
                transform.position = _initialPos;
            else
            {
                //TODO swap
                var tempEntity = _graphData[targetPos].OccupyingEntity;
                _graphData[targetPos].OccupyingEntity = _graphData[_initialPos].OccupyingEntity;
                _graphData[_initialPos].OccupyingEntity = tempEntity;
            }
        }
    }
}