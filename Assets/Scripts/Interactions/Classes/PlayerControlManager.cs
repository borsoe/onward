using Onward.Interactions.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Onward.Interactions.Classes
{
    public class PlayerControlManager : MonoBehaviour
    {

        private Vector2 _mouseScreenPos;
        private IDraggable _currentObjectUnderMouse;

        public Vector3 mouseWorldPos;
        
        public void MousePos(InputAction.CallbackContext context)
        {
            _mouseScreenPos = context.action.ReadValue<Vector2>();
            if (Camera.main != null)
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(_mouseScreenPos.x, _mouseScreenPos.y,
                    0f));
                mouseWorldPos.z = 0f;
            }
            _currentObjectUnderMouse?.OnDrag(mouseWorldPos);
        }


        public void MouseSelect(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_currentObjectUnderMouse == null)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
 
                    if(hitInfo.collider != null)
                    {
                        _currentObjectUnderMouse =
                            hitInfo.collider.gameObject.transform.parent.GetComponent<IDraggable>();
                        _currentObjectUnderMouse.OnDragBegin(mouseWorldPos);
                    }
                }
                else
                {
                    _currentObjectUnderMouse.OnDragEnd(mouseWorldPos);
                    _currentObjectUnderMouse = null;
                }
            }
        }
        
        
    }
}