using UnityEngine;

namespace Units
{
    public class MoveToPointCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 5f;
        private readonly Vector3 _destination;

        public MoveToPointCommand(Vector3 destination)
        {
            _destination = destination;
        }

        public override void BeginCommand(ISelectableUnit unitController)
        {
            base.BeginCommand(unitController);
            NavigationAgent.destination = _destination;
        }

        public override void UpdateCommandState()
        {
            if (UnitHasReachedCurrentDestination(ReachedDestinationThreshold, _destination))
            {
                CompleteCommand();
            }
        }
    }
}