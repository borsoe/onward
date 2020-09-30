using System;
using Onward.AI.Interfaces;
using Onward.Character.Interfaces;
using Onward.Character.MonoBehaviours;
using Onward.Character.ScriptableObjects;
using Onward.Grid.Classes;
using Onward.Grid.MonoBehaviours;
using Onward.Miscellaneous.Enums;
using UnityEngine;
using Zenject;

namespace Onward.Character.Classes
{
    public class Champion : Entity, IAiListener, IAttackAble
    {

        private IAttackAble _target;
        private Node _destination;
        private GraphData _graphData;
        
        public ChampionData championData;

        [Inject]
        public void Construct(GraphData graphData)
        {
            _graphData = graphData;
        }

        public override void GoToLocation(Vector3 pos)
        {
            //TODO change based on the game state
            base.GoToLocation(pos);
        }

        public void Attack()
        {
            //TODO attack speed, animation, etc
            _target.TakeDamage(new Damage
            {
                DamageType = DamageType.Physical,
                Value = championData.attackDamage
            });
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
            return _graphData.BFS(transform.position, championData.attackRange, out _target, out _destination);
        }
        
        public void MoveToward()
        {
            var position = transform.position;
            _graphData[position].OccupyingEntity = null;
            _graphData[position].NexNode.OccupyingEntity = this;
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
            championData.health -= damage.Value;
        }
    }
}