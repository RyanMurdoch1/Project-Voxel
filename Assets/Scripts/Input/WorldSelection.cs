using UnityEngine;

namespace Input
{
    public struct WorldSelection
    {
        public readonly bool SuccessfulSelection;
        public readonly Transform SelectedTransform;
        public Vector3 SelectionPoint;

        public WorldSelection(bool successfulSelection, Transform selectedTransform, Vector3 selectionPoint)
        {
            SuccessfulSelection = successfulSelection;
            SelectedTransform = selectedTransform;
            SelectionPoint = selectionPoint;
        }
    }
}