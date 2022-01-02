using Interactables;
using UnityEngine;

namespace In_Game_Resource_Storage
{
    public class CollectableStorageSpot : MonoBehaviour
    {
        private CollectableObject _collectableObject;
        private bool _spotIsOccupied;

        public void StoreCollectable(CollectableObject collectableObject)
        {
            _collectableObject = collectableObject;
            Transform collectableTransform;
            (collectableTransform = _collectableObject.transform).SetParent(transform);
            collectableTransform.localPosition = Vector3.zero;
            collectableTransform.rotation = Quaternion.identity;
            _spotIsOccupied = true;
        }

        public bool IsNotOccupied()
        {
            return !_spotIsOccupied;
        }
    }
}