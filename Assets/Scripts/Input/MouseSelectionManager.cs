using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IWorldTargetSelector))]
public class MouseSelectionManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction _select, _interact;

    private IWorldTargetSelector _worldTargetSelector;
    private ISelectableUnit _selectedUnit;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player");
        _worldTargetSelector = GetComponent<IWorldTargetSelector>();
        _select = map.FindAction("Select");
        _interact = map.FindAction("Interact");
        _select.performed += OnSelect;
        _interact.performed += OnInteract;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        var selection = _worldTargetSelector.ReturnTarget();
        if (!selection.SuccessfulSelection) return;
        
        if (selection.SelectedTransform.CompareTag(WorldTagHelper.Agent))
        {
            TrySelectHitAgent(selection.SelectedTransform);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        var selection = _worldTargetSelector.ReturnTarget();
        if (!selection.SuccessfulSelection) return;
        
        if (selection.SelectedTransform.CompareTag(WorldTagHelper.Destination) && _selectedUnit != null)
        {
            _selectedUnit.IssueDirection(selection.SelectionPoint, Input.GetKey(KeyCode.LeftShift));
        }
    }

    private void TrySelectHitAgent(Component hit)
    {
        _selectedUnit?.Unselect();
        hit.TryGetComponent(out _selectedUnit);
        _selectedUnit.Select();
    }

    private void OnDisable()
    {
        _select.performed -= OnSelect;
        _interact.performed -= OnInteract;
    }
}