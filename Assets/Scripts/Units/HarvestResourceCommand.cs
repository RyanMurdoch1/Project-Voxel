using Interactables;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class HarvestResourceCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 2f;
        private readonly HarvestableObject _harvestableObject;
        private NavMeshAgent _navigationAgent;
        private bool _hasBeganHarvesting;

        public HarvestResourceCommand(HarvestableObject harvestableObj)
        {
            _harvestableObject = harvestableObj;
        }

        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            _navigationAgent = Unit.GetUnitNavigationAgent();
            _navigationAgent.destination = _harvestableObject.transform.position;
            Debug.Log("Harvest Action Issue to " + unitController.gameObject.name);
        }

        public override void UpdateCommandState()
        {
            if (_harvestableObject.HasBeenHarvested() && !UnitHasReachedCurrentDestination())
            {
                CompleteCommand();
                return;
            }

            if (!UnitHasReachedCurrentDestination() || _hasBeganHarvesting) return;
            Debug.Log("Began Harvesting " + Unit.gameObject.name);
            _harvestableObject.BeginHarvesting(Unit);
            _hasBeganHarvesting = true;
        }

        public override void CancelCommand()
        {
            if (_hasBeganHarvesting && !_harvestableObject.HasBeenHarvested())
            {
                _harvestableObject.CancelHarvesting(Unit);
            }
        }
        
        private bool UnitHasReachedCurrentDestination() => _navigationAgent.remainingDistance < ReachedDestinationThreshold;
    }
}