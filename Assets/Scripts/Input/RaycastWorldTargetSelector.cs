using UnityEngine;

namespace Input
{
    public class RaycastWorldTargetSelector : MonoBehaviour, IWorldTargetSelector
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public WorldSelection ReturnTarget()
        {
            var ray = _camera.ScreenPointToRay(InputManager.GetCursorPosition());
            if (!Physics.Raycast(ray, out var hit))
            {
                return new WorldSelection(false, null, Vector3.zero);
            }

            return new WorldSelection(true, hit.transform, hit.point);
        }
    }
}