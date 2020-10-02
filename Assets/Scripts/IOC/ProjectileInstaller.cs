using Onward.Character.MonoBehaviours;
using UnityEngine;
using Zenject;

namespace Onward.IOC
{
    public class ProjectileInstaller : MonoInstaller
    {

        public SpriteRenderer projectileSprite;
        public Sprite defaultSprite;
        public float defaultSpeed;
        public float offset;
        
        public override void InstallBindings()
        {
            Container.Bind<SpriteRenderer>().FromInstance(projectileSprite).AsSingle();
            Container.Bind<Sprite>().FromInstance(defaultSprite).AsSingle();
            Container.Bind<float>().WithId("speed").FromInstance(defaultSpeed).AsCached();
            Container.Bind<float>().WithId("offset").FromInstance(offset).AsCached();
        }
    }
}