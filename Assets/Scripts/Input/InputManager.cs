using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction _select, _interact, _queue;
    
    public delegate void SelectButtonPressed();
    public static event SelectButtonPressed OnSelection;
    public delegate void InteractButtonPressed();
    public static event InteractButtonPressed OnInteraction;

    public static bool QueuingActive;

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
        _interact.performed += OnInteract;
        QueuingActive = false;
    }

    private static void QueueDown(InputAction.CallbackContext context) => QueuingActive = true;

    private static void QueueUp(InputAction.CallbackContext context) => QueuingActive = false;

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