using System.Net.NetworkInformation;
using Onward.Character.MonoBehaviours;
using Onward.Game.MonoBehaviours;
using Onward.Grid.MonoBehaviours;
using Onward.Interactions.Interfaces;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.Interactions.MonoBehaviours
{
    public class DraggableCharacter : MonoBehaviour, IDraggable
    {
        private Tilemap _allowedTiles;
        private Vector3 _initialPos;
        private GraphData _graphData;
        private Entity _entity;
        private GameManager _gameManager;
        
        [Inject]
        public void Construct([Inject(Id = "ally")]Tilemap allowedTiles, GraphData graphData,
            Entity entity, GameManager gameManager)
        {
            _allowedTiles = allowedTiles;
            _graphData = graphData;
            _entity = entity;
            _gameManager = gameManager;
        }
        
        public void OnDragBegin(Vector3 pos)
        {
            if (_Guard()) return;
            _initialPos = transform.position;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position = pos;
        }

        private bool _Guard()
        {
            return _gameManager.isStateCombat || _entity.faction == Faction.GameSide;
        }

        public void OnDrag(Vector3 pos)
        {
            if (_Guard()) return;
            transform.position = pos;
        }

        public void OnDragEnd(Vector3 pos)
        {
            if (_Guard()) return;
            var cellPos = _allowedTiles.WorldToCell(pos);
            var targetPos =
                _allowedTiles.HasTile(cellPos) ? _allowedTiles.GetCellCenterWorld(cellPos) : _initialPos;
            if (_graphData[targetPos].OccupyingEntity == null)
            {
                _graphData[targetPos].OccupyingEntity = _entity;
                _graphData[_initialPos].OccupyingEntity = null;
            }else if (_graphData[targetPos].OccupyingEntity == _entity)
                transform.position = _initialPos;
            else
            {
                var tempEntity = _graphData[targetPos].OccupyingEntity;
                _graphData[targetPos].OccupyingEntity = _graphData[_initialPos].OccupyingEntity;
                _graphData[_initialPos].OccupyingEntity = tempEntity;
            }
        }
    }
}