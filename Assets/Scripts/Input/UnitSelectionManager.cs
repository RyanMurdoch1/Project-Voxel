using System.Collections.Generic;
using Helpers;
using Interactables;
using Units;
using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(IWorldTargetSelector))]
    public class UnitSelectionManager : MonoBehaviour
    {
        private Color _selectionColor, _selectionBorderColor;
        private IWorldTargetSelector _worldTargetSelector;
        private List<ISelectableUnit> _selectedUnits;
        private bool _isSelecting;
        private Vector2 _startSelectionScreenPosition;
        
        private void OnEnable()
        {
            _selectedUnits = new List<ISelectableUnit>();
            _selectionColor = new Color(0.5f, 1f, 0.4f, 0.2f);
            _selectionBorderColor = new Color(0.5f, 1f, 0.4f);
            _worldTargetSelector = GetComponent<IWorldTargetSelector>();
            InputManager.OnSelection += OnSelect;
            InputManager.OnInteraction += OnInteract;
            InputManager.OnSelectionHeld += OnSelectionHeld;
        }

        private void OnSelectionHeld(bool isHeld)
        {
            if (isHeld)
            {
                _startSelectionScreenPosition = InputManager.GetCursorPosition();
            }
            else if (InputManager.GetCursorPosition() != _startSelectionScreenPosition)
            {
                DeselectAllUnits();
                SelectUnitsInSelectionRect();
            }
            _isSelecting = isHeld;
        }

        private void OnSelect()
        {
            if (!WasSuccessfulSelection(out var selection)) return;
        
            if (selection.SelectedTransform.CompareTag(WorldTagHelper.Unit))
            {
                TrySelectHitAgent(selection.SelectedTransform);
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
        
        private void OnGUI()
        {
            if (!_isSelecting) return;
            var rect = ScreenSelectionHelper.GetScreenRect(_startSelectionScreenPosition, InputManager.GetCursorPosition());
            ScreenSelectionHelper.DrawScreenRect(rect, _selectionColor);
            ScreenSelectionHelper.DrawScreenRectBorder(rect, 1, _selectionBorderColor);
        }
        
        private void SelectUnitsInSelectionRect()
        {
            var selectionBounds = ScreenSelectionHelper.GetViewportBounds(Camera.main, _startSelectionScreenPosition, InputManager.GetCursorPosition());
            var selectableUnits = GameObject.FindGameObjectsWithTag(WorldTagHelper.Unit);
            foreach (var unit in selectableUnits)
            {
                var inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position));
                if (!inBounds) continue;
                var selectableUnit = unit.GetComponent<ISelectableUnit>();
                _selectedUnits.Add(selectableUnit);
                selectableUnit.Select();
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