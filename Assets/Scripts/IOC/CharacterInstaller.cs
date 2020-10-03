using System.ComponentModel;
using Onward.Character.Classes;
using Onward.Character.MonoBehaviours;
using Onward.Character.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class CharacterInstaller : MonoInstaller
    {
        public Gradient gradient;
        public Transform bar;
        public SpriteRenderer spriteRenderer;
        public ChampionData championData;
        public override void InstallBindings()
        {
            Container.Bind<Entity>().FromComponentOnRoot().AsSingle();
            Container.Bind<Gradient>().FromInstance(gradient).AsSingle();
            Container.Bind<Transform>().FromInstance(bar).AsSingle();
            Container.Bind<SpriteRenderer>().FromInstance(spriteRenderer).AsSingle();
            Container.Bind<HealthBar>().FromNew().AsSingle();
            Container.Bind<HealthComponent>().FromNew().AsSingle();
            Container.Bind<float>().FromInstance(championData.maxHealth).AsSingle();
        }
    }
}