using System.ComponentModel;
using Onward.Character.MonoBehaviours;
using UnityEngine.Tilemaps;
using Zenject;

namespace Onward.IOC
{
    public class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Entity>().FromComponentOnRoot().AsSingle();
        }
    }
}