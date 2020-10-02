using Onward.AI.Classes;
using Onward.AI.Interfaces;
using Onward.Character.Classes;
using Onward.Character.MonoBehaviours;
using Onward.Game.Enums;
using Onward.Game.MonoBehaviours;
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
        public Champion champ1;
        public Champion champ2;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AiManager>().AsSingle().NonLazy();
            Container.Bind<IAiListener>().To<Champion>().FromComponentsInHierarchy().AsSingle();
            
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Tilemap>().WithId("ally").FromInstance(allyTileMap).AsCached();
            Container.Bind<Tilemap>().WithId("enemy").FromInstance(enemyTileMap).AsCached();
            Container.Bind<Entity>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<GraphData>().FromInstance(graphData).AsSingle();
            Container.Bind<UnityEngine.Grid>().FromInstance(grid).AsSingle();
        }
    }
}