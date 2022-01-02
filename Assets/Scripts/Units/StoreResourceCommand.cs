using In_Game_Resource_Storage;
using UnityEngine;

namespace Units
{
    public class StoreCollectableCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 4f;
        private ResourceStorageBuilding _targetStorageBuilding;
        private Vector3 _storageDestination;
        
        public override void BeginCommand(ISelectableUnit unitController)
        {
            base.BeginCommand(unitController);
            var storageManager = Object.FindObjectsOfType<ResourceStorageManager>();
            if (storageManager is null)
            {
                CancelCommand();
                return;
            }
            if (storageManager[0].FindAndReturnClosestStorageBuilding(unitController.GetUnitPosition(), out var closestStorage))
            {
                _targetStorageBuilding = closestStorage;
                _storageDestination = _targetStorageBuilding.transform.position;
                NavigationAgent.destination =  _storageDestination;
            }
            else
            {
                CancelCommand();
            }
        }

        public override void UpdateCommandState()
        {
            if (!UnitHasReachedCurrentDestination(ReachedDestinationThreshold, _storageDestination)) return;
            Unit.DistributeCollectable(_targetStorageBuilding);
            CompleteCommand();
        }

        public override void CancelCommand()
        {
            Unit.DropCollectable();
        }
    }
}