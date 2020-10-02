using System;
using Onward.Character.Interfaces;
using Onward.Character.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.Miscellaneous.MonoBehaviours
{
    public class AttackProjectile: MonoBehaviour, IPoolable<Sprite, Damage, float, IAttackAble, Entity, IMemoryPool>, IDisposable
    {
        private SpriteRenderer _sprite;
        private Sprite _projectileSprite;
        private Damage _damage;
        private float _speed;
        private IAttackAble _target;
        private Vector3 _targetPos;
        private float _offset;

        private IMemoryPool _pool;

        [Inject]
        public void Construct(SpriteRenderer sprite, Sprite defaultSprite,
            [Inject(Id = "speed")] float defaultSpeed, [Inject(Id = "offset")] float offset)
        {
            _sprite = sprite;
            _offset = offset;
            _Reset(defaultSprite, null, defaultSpeed, null, null);
        }

        private void _Reset(Sprite projectileSprite, Damage damage, float speed, IAttackAble target, Entity shooter)
        {
            _projectileSprite = projectileSprite != null ? projectileSprite : _projectileSprite;
            _sprite.sprite = _projectileSprite;
            _damage = damage ?? _damage;
            _speed = !float.IsNaN(speed) ? speed : _speed;
            _target = target ?? _target;
            _targetPos = target?.GetPosition() ?? Vector3.zero;
            transform.position = shooter != null ?shooter.transform.position : Vector3.zero;
            if (target != null)
            {
                // Debug.Log($"my owner is {shooter.name} and my target is {((MonoBehaviour) target).name}");
                // Debug.Log($"hence, my position is {transform.position} and I have to go toward {_targetPos} with speed {_speed}");
            }
        }
        
        
        public void Dispose()
        {
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            _pool = null;
            _targetPos = Vector3.zero;
        }

        public void OnSpawned(Sprite p1, Damage p2, float p3, IAttackAble p4, Entity shooter, IMemoryPool p5)
        {
            _Reset(p1, p2, p3, p4, shooter);
            _pool = p5;
        }


        private void Update()
        {
            var transform1 = transform;
            var position = transform1.position;
            position += (_targetPos - position).normalized * (_speed * Time.deltaTime);
            transform1.position = position;
            transform1.right = (_targetPos - position).normalized;
            if ((_targetPos - transform.position).magnitude <= _offset)
            {
                _target.TakeDamage(_damage);
                Dispose();
            }
        }

        public class Factory: PlaceholderFactory<Sprite, Damage, float, IAttackAble, Entity, AttackProjectile>
        {
        }
        
        // public class Pool : MonoMemoryPool<SpriteRenderer, Damage, float, IAttackAble, AttackProjectile>
        // {
        //     protected override void Reinitialize(SpriteRenderer p1, Damage p2, float p3, IAttackAble p4,
        //         AttackProjectile item)
        //     {
        //         item._Reset(p1, p2, p3, p4);
        //     }
        // }
    }
}