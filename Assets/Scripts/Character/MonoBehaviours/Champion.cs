using System.Collections;
using Onward.AI.Classes;
using Onward.AI.Interfaces;
using Onward.Character.Classes;
using Onward.Character.Interfaces;
using Onward.Character.ScriptableObjects;
using Onward.Game.MonoBehaviours;
using Onward.Grid.MonoBehaviours;
using Onward.Miscellaneous.Enums;
using Onward.Miscellaneous.MonoBehaviours;
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
        
        private AttackProjectile.Factory _attackProjectileFactory;


        public ChampionData championData;
        [SerializeField] private float distanceOffset;

        [Inject]
        public void Construct(GraphData graphData, GameManager gameManager, AiManager aiManager, HealthComponent healthComponent,
            AttackProjectile.Factory factory)
        {
            _graphData = graphData;
            _gameManager = gameManager;
            _aiManager = aiManager;
            _healthComponent = healthComponent;
            _attackProjectileFactory = factory;
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
            StartCoroutine(AttackAction());
        }
        
        public bool CheckTarget()
        {
            return _IsTargetAvailable();
        }

        private bool _IsTargetAvailable()
        {
            return _target != null && _graphData.Distance(transform.position, _target.GetPosition()) <=
                   championData.attackRange;
        }

        public bool FindPathToNearestTarget()
        {
            return _graphData.BFS(transform.position, championData.attackRange, out _target);
        }
        
        public void MoveToward()
        {
            var position = transform.position;
            // Debug.Log($"this node pos is: {_graphData[position].Location} next node pos is: {_graphData[position].NexNode.Location}");
            _graphData[position].OccupyingEntity = null;
            _graphData[position].NextNode.OccupyingEntity = this;
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
            //TODO sadta chiz!!!
            _healthComponent.Health -= damage.Value;
        }

        private void RangeAttack()
        {
            Debug.Log($"attacking from side {this.faction}");
            var attackProjectile = _attackProjectileFactory.Create(championData.attackProjectileSprite, new Damage
            {
                DamageType = DamageType.Physical,
                Value = championData.attackDamage
            }, championData.rangeAttackTravelSpeed, _target, this);
            attackProjectile.transform.SetParent(null);
        }

        #endregion

        #region Co-Routins

        private IEnumerator Move(Vector3 to)
        {
            var delta = (to - transform.position).magnitude;
            var timeToTravers = 100 / championData.moveSpeed;
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

        private IEnumerator AttackAction()
        {
            // Debug.Log($"character {name} is attacking {((MonoBehaviour)_target).name}");
            float waitTime = 1f / championData.attackSpeed;
            yield return new WaitForSeconds(waitTime);
            
            if (championData.attackRange > 1)
            {
                RangeAttack();
            }else
                _target.TakeDamage(new Damage
                {
                    DamageType = DamageType.Physical,
                    Value = championData.attackDamage
                });
            // _aiManager.NewListener = this;
            _aiManager.SetNewListener(this);
            yield return new WaitForEndOfFrame();
        }

        #endregion
    }
}