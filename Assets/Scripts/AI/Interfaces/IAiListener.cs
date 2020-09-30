namespace Onward.AI.Interfaces
{
    public interface IAiListener
    {
        void Attack();
        bool CheckTarget();
        bool FindPathToNearestTarget();
        void MoveToward();
    }
}