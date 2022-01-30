using System.Collections.Generic;
using Environment_Builder;
using UnityEngine;

namespace Editor
{
    public static class SceneViewObjectPositioner
    {
        public static bool IsPlacingObject;
        private static GameObject _currentObjectToSpawn, _objectBeingPlaced;
        private static EnvironmentObject _currentEnvironmentObject;
        private static MeshRenderer _renderer;
        private static Material _placedMaterial;
        private static Collider _collider;
        private static readonly Color Red = new Color(1, 0, 0, 0.5f);
        private static readonly Color Green = new Color(0, 1, 0, 0.5f);
        private static string UnitObjectName = "[Units]", ResourceObjectName = "[Resources]";
        private static List<GameObject> _placedObjects = new List<GameObject>();
        
        public static void BeginObjectPlacement(RaycastHit hit, EnvironmentObject environmentObject)
        {
            _currentEnvironmentObject = environmentObject;
            _currentObjectToSpawn = _currentEnvironmentObject.objectPrefab;
            _objectBeingPlaced = Object.Instantiate(_currentObjectToSpawn);
            _renderer = _objectBeingPlaced.GetComponent<MeshRenderer>();
            _collider = _objectBeingPlaced.GetComponent<Collider>();
            _collider.enabled = false;
            _placedMaterial = _renderer.sharedMaterial;
            _renderer.material = _currentEnvironmentObject.placementMaterial;
            _objectBeingPlaced.transform.position = GetObjectPlacementPosition(hit);
            IsPlacingObject = true;
        }
        
        public static void RenderObjectPlacement(RaycastHit hit)
        {
            var placementPosition = GetObjectPlacementPosition(hit);
            _objectBeingPlaced.transform.position = placementPosition;
            SetPlacementObjectMaterial(IsPlaceable());
        }
        
        public static void PlaceObject()
        {
            _renderer.material = _placedMaterial;
            _collider.enabled = true;
            
            switch (_currentEnvironmentObject.objectType)
            {
                case EnvironmentObjectType.Resource:
                    SetParentObject(ResourceObjectName);
                    break;
                case EnvironmentObjectType.Unit:
                    SetParentObject(UnitObjectName);
                    break;
            }

            _placedObjects.Add(_objectBeingPlaced);
            IsPlacingObject = false;
        }

        public static bool CanUndo()
        {
            return _placedObjects.Count > 0;
        }

        public static void Undo()
        {
            if (_placedObjects.Count <= 0) return;
            var removeObject = _placedObjects[_placedObjects.Count - 1];
            _placedObjects.Remove(removeObject);
            Object.DestroyImmediate(removeObject);
        }

        public static void RotateObjectLeft()
        {
            RotateObject(1);
        }

        public static void RotateObjectRight()
        {
            RotateObject(-1);
        }

        private static void RotateObject(float value)
        {
            if (_objectBeingPlaced == null) return;
            _objectBeingPlaced.transform.Rotate(0, value, 0);
        }

        private static void SetParentObject(string parentName)
        {
            if (!GameObject.Find(parentName))
            {
                var newHolder = new GameObject
                {
                    name = parentName
                };
            }
                
            _objectBeingPlaced.transform.SetParent(GameObject.Find(parentName).transform);
        }
        
        private static Vector3 GetObjectPlacementPosition(RaycastHit hit)
        {
            return new Vector3(hit.point.x, hit.point.y + _currentEnvironmentObject.placementYOffset, hit.point.z);
        }
        
        private static void SetPlacementObjectMaterial(bool isPlaceable)
        {
            _currentEnvironmentObject.placementMaterial.color = isPlaceable ? Green : Red;
        }
        
        public static bool IsPlaceable()
        {
            var colliders = Physics.OverlapSphere(_objectBeingPlaced.transform.position, _objectBeingPlaced.transform.localScale.x / 1.5f);
            
            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider.gameObject.name == "Plane" || collider.gameObject == _objectBeingPlaced)
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}