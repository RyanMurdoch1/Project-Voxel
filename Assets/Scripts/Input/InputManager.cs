using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        private InputAction _select, _interact, _queue;
    
        public delegate void SelectButtonPressed();
        public static event SelectButtonPressed OnSelection;
        public delegate void InteractButtonPressed();
        public static event InteractButtonPressed OnInteraction;
        public delegate void SelectionIsHeld(bool isHeld);
        public static event SelectionIsHeld OnSelectionHeld;

        public static bool QueueingActive;

        public static Vector2 GetCursorPosition()
        {
            return Mouse.current.position.ReadValue();
        }
    
        private void OnEnable()
        {
            var map = inputActions.FindActionMap(InputActionHelper.PlayerInputMap);
            _select = map.FindAction(InputActionHelper.SelectAction);
            _interact = map.FindAction(InputActionHelper.InteractAction);
            _queue = map.FindAction(InputActionHelper.QueueAction);
            _queue.started += QueueDown;
            _queue.canceled += QueueUp;
            _select.performed += OnSelect;
            _select.started += SelectionStarted;
            _select.canceled += SelectionEnded;
            _interact.performed += OnInteract;
            QueueingActive = false;
        }

        private static void SelectionStarted(InputAction.CallbackContext context)
        {
            OnSelectionHeld?.Invoke(true);
        }

        private static void SelectionEnded(InputAction.CallbackContext context)
        {
            OnSelectionHeld?.Invoke(false);
        }

        private static void QueueDown(InputAction.CallbackContext context) => QueueingActive = true;

        private static void QueueUp(InputAction.CallbackContext context) => QueueingActive = false;

        private static void OnSelect(InputAction.CallbackContext context)
        {
            OnSelection?.Invoke();
        }

        private static void OnInteract(InputAction.CallbackContext context)
        {
            OnInteraction?.Invoke();
        }

        private void OnDisable()
        {
            _select.performed -= OnSelect;
            _interact.performed -= OnInteract;
            _queue.started -= QueueDown;
            _queue.canceled -= QueueUp;
        }
    }
}