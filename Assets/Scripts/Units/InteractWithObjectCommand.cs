using Interactables;

namespace Units
{
    public class InteractWithObjectCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 4f;
        private readonly IInteractableObject _interactiveObject;
        private bool _hasBeganInteraction;

        public InteractWithObjectCommand(IInteractableObject harvestableObj)
        {
            _interactiveObject = harvestableObj;
        }

        public override void BeginCommand(ISelectableUnit unitController)
        {
            base.BeginCommand(unitController);
            NavigationAgent.destination = _interactiveObject.GetUnitDestination();
        }

        public override void UpdateCommandState()
        {
            if (IsNoLongerValidInteraction())
            {
                CompleteCommand();
                return;
            }

            if (_hasBeganInteraction || !UnitHasReachedCurrentDestination(ReachedDestinationThreshold, NavigationAgent.destination)) return;
            _interactiveObject.BeginInteraction(Unit);
            _hasBeganInteraction = true;
        }
        
        public override void CancelCommand()
        {
            if (_hasBeganInteraction && !_interactiveObject.IsValidInteraction())
            {
                _interactiveObject.CancelInteraction(Unit);
            }
        }
        
        private bool IsNoLongerValidInteraction()
        {
            return !_interactiveObject.IsValidInteraction() && !_hasBeganInteraction;
        }
    }
}