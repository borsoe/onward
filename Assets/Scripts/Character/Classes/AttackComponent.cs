using System.Collections;
using Onward.AI.Classes;
using Onward.AI.Interfaces;
using Onward.Character.Interfaces;
using Onward.Character.MonoBehaviours;
using Onward.Miscellaneous.Enums;
using Onward.Miscellaneous.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.Character.Classes
{
    /// <summary>
    /// The component that is responsible for any entities attack behaviours and data
    /// </summary>
    public class AttackComponent
    {
        #region setup

        private readonly float _attackSpeed;
        private readonly float _attackDamage;
        private readonly AiManager _aiManager;
        private readonly AttackProjectile.Factory _attackProjectileFactory;
        private readonly Sprite _attackProjectileSprite;
        private readonly float _rangeAttackTravelSpeed;

        [Inject]
        public AttackComponent(AiManager aiManager, int attackRange,
            [Inject(Id = "attackSpeed")] float attackSpeed,[Inject(Id = "attackDamage")] float attackDamage,
            Sprite attackProjectileSprite,[Inject(Id = "travelSpeed")] float rangeAttackTravelSpeed,
            AttackProjectile.Factory factory)
        {
            _aiManager = aiManager;
            AttackRange = attackRange;
            _attackSpeed = attackSpeed;
            _attackDamage = attackDamage;
            _attackProjectileSprite = attackProjectileSprite;
            _rangeAttackTravelSpeed = rangeAttackTravelSpeed;
            _attackProjectileFactory = factory;
        }

        #endregion

        #region properties

        public int AttackRange { get; private set; }
        

        #endregion
        
        #region methodes

        private void _RangeAttack(IAttackAble target, Entity entity)
        {
            var attackProjectile = _attackProjectileFactory.Create(_attackProjectileSprite, new Damage
            {
                DamageType = DamageType.Physical,
                Value = _attackDamage
            }, _rangeAttackTravelSpeed, target, entity);
            attackProjectile.transform.SetParent(null);
        }
        

        #endregion

        #region co-routines

        public IEnumerator AttackAction(IAttackAble target, IAiListener listener)
        {
            float waitTime = 1f / _attackSpeed;
            yield return new WaitForSeconds(waitTime);
            
            if (AttackRange > 1)
            {
                _RangeAttack(target, (Entity) listener);
            }else
                target.TakeDamage(new Damage
                {
                    DamageType = DamageType.Physical,
                    Value = _attackDamage
                });
            _aiManager.SetNewListener(listener);
            yield return new WaitForEndOfFrame();
        }
        

        #endregion 
    }
}