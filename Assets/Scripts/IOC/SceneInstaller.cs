using Onward.Character.MonoBehaviours;
using Onward.Grid.MonoBehaviours;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class SceneInstaller : MonoInstaller
    {
        [FormerlySerializedAs("tileMap")] 
        public Tilemap allyTileMap;
        public Tilemap enemyTileMap; 
        public GraphData graphData;
        public UnityEngine.Grid grid;
        public override void InstallBindings()
        {
            Container.Bind<Tilemap>().WithId("ally").FromInstance(allyTileMap).AsCached();
            Container.Bind<Tilemap>().WithId("enemy").FromInstance(enemyTileMap).AsCached();
            Container.Bind<Entity>().FromComponentsInHierarchy().AsTransient();
            Container.Bind<GraphData>().FromInstance(graphData).AsSingle();
            Container.Bind<UnityEngine.Grid>().FromInstance(grid).AsSingle();
        }
    }
}