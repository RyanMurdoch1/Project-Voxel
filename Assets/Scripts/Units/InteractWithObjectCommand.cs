using Interactables;
using UnityEngine;

namespace Units
{
    public class InteractWithObjectCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 2f;
        private readonly IInteractableObject _interactiveObject;
        private bool _hasBeganInteraction;

        public InteractWithObjectCommand(IInteractableObject harvestableObj)
        {
            _interactiveObject = harvestableObj;
        }

        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            NavigationAgent.destination = _interactiveObject.GetWorldLocation();
        }

        public override void UpdateCommandState()
        {
            if (IsNoLongerValidInteraction())
            {
                CompleteCommand();
                return;
            }

            if (!UnitHasReachedCurrentDestination(ReachedDestinationThreshold) || _hasBeganInteraction) return;
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