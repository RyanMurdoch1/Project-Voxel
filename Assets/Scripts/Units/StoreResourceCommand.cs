using In_Game_Resource_Storage;
using UnityEngine;

namespace Units
{
    public class StoreCollectableCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 2f;
        private ResourceStorageBuilding _targetStorageBuilding;
        private readonly Harvestable _resource;
        private readonly int _units;

        public StoreCollectableCommand(Harvestable resource, int units)
        {
            _resource = resource;
            _units = units;
        }
        
        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            var storageManager = Object.FindObjectsOfType<ResourceStorageManager>();
            if (storageManager is null)
            {
                CancelCommand();
                return;
            }
            if (storageManager[0].FindAndReturnClosestStorageBuilding(unitController.transform.position, out var closestStorage))
            {
                _targetStorageBuilding = closestStorage;
                NavigationAgent.destination =  _targetStorageBuilding.transform.position;
            }
            else
            {
                CancelCommand();
            }
        }

        public override void UpdateCommandState()
        {
            if (!UnitHasReachedCurrentDestination(ReachedDestinationThreshold)) return;
            _targetStorageBuilding.DepositResources(_resource, _units);
            Unit.DistributeCollectable();
            CompleteCommand();
        }

        public override void CancelCommand()
        {
            Unit.DropCollectable();
        }
    }
}