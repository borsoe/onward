



using UnityEngine;

namespace Onward.Interactions.Interfaces
{
    public interface IDraggable
    {
        void OnDragBegin(Vector3 pos);
        void OnDrag(Vector3 pos);
        void OnDragEnd(Vector3 pos);
    }
}