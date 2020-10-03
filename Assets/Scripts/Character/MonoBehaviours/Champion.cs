using System.Collections;
using Onward.AI.Classes;
using Onward.AI.Interfaces;
using Onward.Character.Classes;
using Onward.Character.Interfaces;
using Onward.Game.MonoBehaviours;
using Onward.Grid.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.Character.MonoBehaviours
{
    /// <summary>
    /// The Identity class for Champions(Characters)
    /// </summary>
    public class Champion : Entity, IAiListener, IAttackAble
    {

        #region Setup

        private IAttackAble _target;
        private GraphData _graphData;
        private GameManager _gameManager;
        private AiManager _aiManager;
        private HealthComponent _healthComponent;
        private AttackComponent _attackComponent;
        private MoveComponent _moveComponent;
        private EntitySpriteHandler _entitySpriteHandler;

        [SerializeField] private float distanceOffset;

        [Inject]
        public void Construct(GraphData graphData, GameManager gameManager, AiManager aiManager, 
            HealthComponent healthComponent, AttackComponent attackComponent, MoveComponent moveComponent, EntitySpriteHandler entitySpriteHandler)
        {
            _graphData = graphData;
            _gameManager = gameManager;
            _aiManager = aiManager;
            _healthComponent = healthComponent;
            _attackComponent = attackComponent;
            _moveComponent = moveComponent;
            _entitySpriteHandler = entitySpriteHandler;
        }

        #endregion

        #region Methods

        public override void GoToLocation(Vector3 pos)
        {
            if (!_gameManager.isStateCombat)
            {
                base.GoToLocation(pos);
            }
            else
            {
                StartCoroutine(Move(pos));
            }
        }

        public void Attack()
        {
            StartCoroutine(_attackComponent.AttackAction(_target, this));
        }
        
        public bool CheckTarget()
        {
            return _IsTargetAvailable();
        }

        private bool _IsTargetAvailable()
        {
            return _target != null && _graphData.Distance(transform.position, _target.GetPosition()) <=
                   _attackComponent.AttackRange;
        }

        public bool FindPathToNearestTarget()
        {
            return _graphData.BFS(transform.position, _attackComponent.AttackRange, out _target);
        }
        
        public void MoveToward()
        {
            _moveComponent.MoveToNextTile(_graphData[transform.position]);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public bool CanBeAttackedBy(Entity attacker)
        {
            return faction != attacker.faction;
        }

        public void TakeDamage(Damage damage)
        {
            StartCoroutine(_entitySpriteHandler.Flicker(Color.red, 0.1f, 0.3f));
            //TODO sadta chiz!
            _healthComponent.Health -= damage.Value;
        }

        #endregion

        #region Co-Routins

        private IEnumerator Move(Vector3 to)
        {
            var delta = (to - transform.position).magnitude;
            var timeToTravers = 100 / _moveComponent.MoveSpeed;
            // Debug.Log($"pos is: {transform.position} and to is: {to}");
            while ((transform.position - to).magnitude >= distanceOffset)
            {
                // Debug.Log((transform.position - to).magnitude);
                var position = transform.position;
                var normal = (to - position).normalized;
                var step = timeToTravers / Time.deltaTime;
                position += normal * (delta / step);
                // ReSharper disable once Unity.InefficientPropertyAccess
                transform.position = position;
                yield return new WaitForEndOfFrame();
            }
            // _aiManager.NewListener = this;
            _aiManager.SetNewListener(this);
            yield return new WaitForEndOfFrame();
        }

        #endregion
    }
}