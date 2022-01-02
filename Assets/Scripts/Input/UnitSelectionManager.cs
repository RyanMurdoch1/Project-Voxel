using System.Collections.Generic;
using Helpers;
using Interactables;
using Units;
using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(IWorldTargetSelector))]
    [RequireComponent(typeof(IWorldRectSelector))]
    public class UnitSelectionManager : MonoBehaviour
    {
        private List<ISelectableUnit> _allSelectableUnits;
        private IWorldRectSelector _selectionRectVisualizer;
        private IWorldTargetSelector _worldTargetSelector;
        private List<ISelectableUnit> _selectedUnits;
        
        private void OnEnable()
        {
            Initialize();
            SubscribeToInputManager();
            GetAllSelectableUnits();
        }

        private void GetAllSelectableUnits()
        {
            _allSelectableUnits = new List<ISelectableUnit>();
            var selectableUnits = GameObject.FindGameObjectsWithTag(WorldTagHelper.Unit);
            for (var i = 0; i < selectableUnits.Length; i++)
            {
                _allSelectableUnits.Add(selectableUnits[i].GetComponent<ISelectableUnit>());
            }
        }

        private void SubscribeToInputManager()
        {
            InputManager.OnSelection += OnSelect;
            InputManager.OnInteraction += OnInteract;
            InputManager.OnSelectionHeld += OnSelectionHeld;
        }

        private void Initialize()
        {
            _selectedUnits = new List<ISelectableUnit>();
            _worldTargetSelector = GetComponent<IWorldTargetSelector>();
            _selectionRectVisualizer = GetComponent<IWorldRectSelector>();
        }

        private void OnSelectionHeld(bool isHeld)
        {
            if (isHeld)
            {
                _selectionRectVisualizer.StartDrawingRect();
            }
            else if (_selectionRectVisualizer.HasDrawnSelectionRect())
            {
                DeselectAllUnits();
                _selectedUnits = _selectionRectVisualizer.ReturnUnitsInRect(_allSelectableUnits);
            }
        }

        private void OnSelect()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Unit))
            {
                TrySelectHitUnit(selection.SelectedTransform);
            }
            else if (_selectedUnits.Count != 0)
            {
                DeselectAllUnits();
            }
        }

        private void OnInteract()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Destination) && _selectedUnits.Count != 0)
            {
                for (var i = 0; i < _selectedUnits.Count; i++)
                {
                    _selectedUnits[i].IssueCommand(new MoveToPointCommand(selection.SelectionPoint), InputManager.QueueingActive);
                }
            }

            if (!selection.SelectedTransform.CompareTag(WorldTagHelper.Interactable) || _selectedUnits.Count == 0) return;
            
            if (selection.SelectedTransform.TryGetComponent(typeof(IInteractableObject), out var harvestableObj))
            {
                for (var i = 0; i < _selectedUnits.Count; i++)
                {
                    _selectedUnits[i].IssueCommand(new InteractWithObjectCommand((IInteractableObject)harvestableObj), InputManager.QueueingActive);
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

        private void TrySelectHitUnit(Component hit)
        {
            DeselectAllUnits();
            hit.TryGetComponent(out ISelectableUnit selectedUnit);
            _selectedUnits.Add(selectedUnit);
            selectedUnit.Select();
        }

        private void DeselectAllUnits()
        {
            for (var i = 0; i < _selectedUnits.Count; i++)
            {
                _selectedUnits[i].Unselect();
            }
            _selectedUnits.Clear();
        }

        private void OnDisable()
        {
            InputManager.OnSelection -= OnSelect;
            InputManager.OnInteraction -= OnInteract;
            InputManager.OnSelectionHeld -= OnSelectionHeld;
        }
    }
}