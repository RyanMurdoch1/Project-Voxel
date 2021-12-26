using UnityEngine;

public struct WorldSelection
{
    public bool SuccessfulSelection;
    public Transform SelectedTransform;
    public Vector3 SelectionPoint;

    public WorldSelection(bool successfulSelection, Transform selectedTransform, Vector3 selectionPoint)
    {
        SuccessfulSelection = successfulSelection;
        SelectedTransform = selectedTransform;
        SelectionPoint = selectionPoint;
    }
}