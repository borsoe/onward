using Onward.Character.MonoBehaviours;
using UnityEngine;

namespace Onward.Character.Interfaces
{
    public interface IAttackAble
    {
        Vector3 GetPosition();
        bool CanBeAttackedBy(Entity attacker);
        void TakeDamage(Damage damage);
    }
}