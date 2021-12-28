using Helpers;
using Interactables;
using Units;
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
            else if (_selectedUnit != null)
            {
                _selectedUnit.Unselect();
                _selectedUnit = null;
            }
        }

        private void OnInteract()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Destination) && _selectedUnit != null)
            {
                _selectedUnit.IssueCommand(new MoveToPointCommand(selection.SelectionPoint), InputManager.QueuingActive);
            }
            
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Harvestable) && _selectedUnit != null)
            {
                if (selection.SelectedTransform.TryGetComponent(typeof(HarvestableObject), out var harvestableObj))
                {
                    _selectedUnit.IssueCommand(new HarvestResourceCommand((HarvestableObject)harvestableObj), InputManager.QueuingActive);
                }
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