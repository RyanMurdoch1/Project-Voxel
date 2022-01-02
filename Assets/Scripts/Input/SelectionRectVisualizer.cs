using System.Collections.Generic;
using Helpers;
using Units;
using UnityEngine;

namespace Input
{
    public class SelectionRectVisualizer : MonoBehaviour, IWorldRectSelector
    {
        private readonly Color _selectionColor = new Color(0.5f, 1f, 0.4f, 0.2f);
        private readonly Color _selectionBorderColor = new Color(0.5f, 1f, 0.4f);
        private Camera _camera;
        private bool _isSelecting;
        private Vector2 _startSelectionScreenPosition;
        private List<ISelectableUnit> _selectedUnits;

        private void Awake()
        {
            _camera = Camera.main;
            _selectedUnits = new List<ISelectableUnit>();
        }

        public void StartDrawingRect()
        {
            _startSelectionScreenPosition = InputManager.GetCursorPosition();
            _isSelecting = true;
        }

        public bool HasDrawnSelectionRect()
        {
            _isSelecting = false;
            return _startSelectionScreenPosition != InputManager.GetCursorPosition();
        }

        private void OnGUI()
        {
            if (!_isSelecting) return;
            var rect = ScreenSelectionHelper.GetScreenRect(_startSelectionScreenPosition, InputManager.GetCursorPosition());
            ScreenSelectionHelper.DrawScreenRect(rect, _selectionColor);
            ScreenSelectionHelper.DrawScreenRectBorder(rect, 1, _selectionBorderColor);
        }
        
        public List<ISelectableUnit> ReturnUnitsInRect(List<ISelectableUnit> allUnits)
        {
            _selectedUnits.Clear();
            var selectionBounds = ScreenSelectionHelper.GetViewportBounds(Camera.main, _startSelectionScreenPosition, InputManager.GetCursorPosition());
            for (var i = 0; i < allUnits.Count; i++)
            {
                var unit = allUnits[i];
                var inBounds = selectionBounds.Contains(_camera.WorldToViewportPoint(unit.GetUnitPosition()));
                if (!inBounds) continue;
                _selectedUnits.Add(unit);
                unit.Select();
            }

            return _selectedUnits;
        }
    }
}