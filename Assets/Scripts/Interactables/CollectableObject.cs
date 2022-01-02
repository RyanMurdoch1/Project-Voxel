using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Interactables
{
    public class CollectableObject : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private Rigidbody rigidBody;
        private bool _hasBeenCollected;
        private ISelectableUnit _collectingUnit;
        
        public void BeginInteraction(ISelectableUnit unit)
        {
            _hasBeenCollected = true;
            _collectingUnit = unit;
            _collectingUnit.IssueCommandOverride(new StoreCollectableCommand());
            rigidBody.isKinematic = true;
            unit.ReceiveCollectable(this);
        }

        public void CancelInteraction(ISelectableUnit unit)
        {
            _hasBeenCollected = false;
            transform.SetParent(null);
            rigidBody.isKinematic = false;
        }

        public Vector3 GetUnitDestination()
        {
            return gameObject.transform.position;
        }

        public bool IsValidInteraction()
        {
            return !_hasBeenCollected;
        }
    }
}
