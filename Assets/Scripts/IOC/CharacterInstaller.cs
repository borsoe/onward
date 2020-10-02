using System.ComponentModel;
using Onward.Character.Classes;
using Onward.Character.MonoBehaviours;
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
        public override void InstallBindings()
        {
            Container.Bind<Entity>().FromComponentOnRoot().AsSingle();
            Container.Bind<Gradient>().FromInstance(gradient).AsSingle();
            Container.Bind<Transform>().FromInstance(bar).AsSingle();
            Container.Bind<SpriteRenderer>().FromInstance(spriteRenderer).AsSingle();
            Container.Bind<HealthBar>().FromNew().AsSingle();
        }
    }
}