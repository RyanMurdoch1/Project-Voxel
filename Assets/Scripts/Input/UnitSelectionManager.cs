using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(IWorldTargetSelector))]
    public class UnitSelectionManager : MonoBehaviour
    {
        private IWorldTargetSelector _worldTargetSelector;
        private ISelectableUnit _selectedUnit;

        private void OnEnable()
        {
            _worldTargetSelector = GetComponent<IWorldTargetSelector>();
            InputManager.OnSelection += OnSelect;
            InputManager.OnInteraction += OnInteract;
        }

        private void OnSelect()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Agent))
            {
                TrySelectHitAgent(selection.SelectedTransform);
            }
        }

        private void OnInteract()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Destination) && _selectedUnit != null)
            {
                _selectedUnit.IssueDirection(selection.SelectionPoint, InputManager.QueuingActive);
            }
        }

        private bool WasSuccessfulSelection(out WorldSelection selection)
        {
            var worldSelection = _worldTargetSelector.ReturnTarget();
            if (!worldSelection.SuccessfulSelection)
            {
                selection = worldSelection;
                return false;
            }

            selection = worldSelection;
            return true;
        }

        private void TrySelectHitAgent(Component hit)
        {
            _selectedUnit?.Unselect();
            hit.TryGetComponent(out _selectedUnit);
            _selectedUnit.Select();
        }

        private void OnDisable()
        {
            InputManager.OnSelection -= OnSelect;
            InputManager.OnInteraction -= OnInteract;
        }
    }
}