using UnityEngine;

public class MouseSelectionManager : MonoBehaviour
{
    private Camera _camera;
    private ICommandable _selectedCommandable;

    private void Awake() => _camera = Camera.main;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            HandleSelection(hit);
        }
    }

    private void HandleSelection(RaycastHit hit)
    {
        if (hit.transform.CompareTag(WorldTagHelper.Destination) && _selectedCommandable != null)
        {
            _selectedCommandable.IssueDirection(hit.point, Input.GetKey(KeyCode.LeftShift));
            return;
        }
        
        if (hit.transform.CompareTag(WorldTagHelper.Agent))
        {
            TrySelectHitAgent(hit);
        }
    }

    private void TrySelectHitAgent(RaycastHit hit)
    {
        _selectedCommandable?.Unselect();
        hit.transform.TryGetComponent(out _selectedCommandable);
        _selectedCommandable.Select();
    }
}