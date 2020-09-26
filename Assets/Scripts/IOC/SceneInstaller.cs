using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class SceneInstaller : MonoInstaller
    {
        public Tilemap tileMap;
        public override void InstallBindings()
        {
            Container.Bind<Tilemap>().FromInstance(tileMap).AsSingle();
        }
    }
}