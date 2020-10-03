using System.ComponentModel;
using Onward.Character.Classes;
using Onward.Character.MonoBehaviours;
using Onward.Character.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class CharacterInstaller : MonoInstaller
    {
        public Gradient gradient;
        public Transform bar;
        [FormerlySerializedAs("spriteRenderer")] public SpriteRenderer healthBarRenderer;
        public SpriteRenderer entityRenderer;
        [FormerlySerializedAs("_championData")] [Inject]
        public ChampionData championData;
        public override void InstallBindings()
        {
            Container.Bind<Entity>().FromComponentInParents().AsSingle();
            Container.Bind<Gradient>().FromInstance(gradient).AsSingle();
            Container.Bind<Transform>().FromInstance(bar).AsSingle().WhenInjectedInto<HealthBar>();
            Container.Bind<SpriteRenderer>().FromInstance(healthBarRenderer).AsCached().WhenInjectedInto<HealthBar>();
            Container.Bind<HealthBar>().FromNew().AsSingle();
            Container.Bind<HealthComponent>().FromNew().AsSingle();
            // Container.Bind<int>().FromInstance(championData.attackRange).AsSingle();
            // Container.Bind<float>().WithId("health").FromInstance(championData.maxHealth).AsCached();
            // Container.Bind<float>().WithId("attackSpeed").FromInstance(championData.attackSpeed).AsCached();
            // Container.Bind<float>().WithId("attackDamage").FromInstance(championData.attackDamage).AsCached();
            // Container.Bind<float>().WithId("travelSpeed").FromInstance(championData.rangeAttackTravelSpeed).AsCached();
            // Container.Bind<Sprite>().FromInstance(championData.attackProjectileSprite).AsSingle()
            //     .WhenInjectedInto<AttackComponent>();
            Container.Bind<AttackComponent>().FromNew().AsSingle();
            // Container.Bind<float>().WithId("moveSpeed").FromInstance(championData.moveSpeed).AsCached();
            Container.Bind<MoveComponent>().FromNew().AsSingle();
            Container.Bind<SpriteRenderer>().FromInstance(entityRenderer).AsCached()
                .WhenInjectedInto<EntitySpriteHandler>();
            // Container.Bind<Sprite>().FromInstance(championData.characterSprite).AsCached()
            // .WhenInjectedInto<EntitySpriteHandler>();
            Container.Bind<EntitySpriteHandler>().FromNew().AsSingle();
            // Container.Bind<Faction>().FromInstance(championData.faction).AsSingle();
            Container.BindInstance(championData).AsSingle();
        }
    }
}