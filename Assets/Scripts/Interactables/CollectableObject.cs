using Units;
using UnityEngine;

namespace Interactables
{
    public class CollectableObject : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private Harvestable resourceType;
        [SerializeField] private Rigidbody rigidBody;
        private bool _hasBeenCollected;
        private UnitController _collectingUnit;

        public void BeginInteraction(UnitController unit)
        {
            _hasBeenCollected = true;
            _collectingUnit = unit;
            _collectingUnit.IssueCommandOverride(new StoreCollectableCommand(resourceType, 1));
            rigidBody.isKinematic = true;
            unit.ReceiveCollectable(this);
        }

        public void CancelInteraction(UnitController unit)
        {
            _hasBeenCollected = false;
            transform.SetParent(null);
            rigidBody.isKinematic = false;
        }

        public Vector3 GetWorldLocation()
        {
            return gameObject.transform.position;
        }

        public bool IsValidInteraction()
        {
            return !_hasBeenCollected;
        }
    }
}
