using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class SceneInstaller : MonoInstaller
    {
        [FormerlySerializedAs("tileMap")] public Tilemap allyTileMap;
        public Tilemap enemyTileMap;
        public override void InstallBindings()
        {
            Container.Bind<Tilemap>().WithId("ally").FromInstance(allyTileMap).AsCached();
            Container.Bind<Tilemap>().WithId("enemy").FromInstance(enemyTileMap).AsCached();
        }
    }
}