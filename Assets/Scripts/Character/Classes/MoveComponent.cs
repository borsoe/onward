using Onward.Grid.Classes;
using Zenject;

namespace Onward.Character.Classes
{
    /// <summary>
    /// component that is responsible for entity movement;
    /// </summary>
    public class MoveComponent
    {
        #region setup

        [Inject]
        public MoveComponent([Inject(Id = "moveSpeed")]float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }

        #endregion

        #region properties

        public float MoveSpeed { get; set; }

        #endregion
        
        #region methodes

        public void MoveToNextTile(Node node)
        {
            var current = node.OccupyingEntity;
            node.OccupyingEntity = null;
            node.NextNode.OccupyingEntity = current;
        }

        #endregion
    }
}