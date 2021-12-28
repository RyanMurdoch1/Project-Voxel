using Interactables;

namespace Units
{
    public class HarvestResourceCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 2f;
        private readonly HarvestableObject _harvestableObject;
        private bool _hasBeganHarvesting;

        public HarvestResourceCommand(HarvestableObject harvestableObj)
        {
            _harvestableObject = harvestableObj;
        }

        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            NavigationAgent.destination = _harvestableObject.transform.position;
        }

        public override void UpdateCommandState()
        {
            if (_harvestableObject.HasBeenHarvested() && !UnitHasReachedCurrentDestination(ReachedDestinationThreshold))
            {
                CompleteCommand();
                return;
            }

            if (!UnitHasReachedCurrentDestination(ReachedDestinationThreshold) || _hasBeganHarvesting) return;
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
    }
}