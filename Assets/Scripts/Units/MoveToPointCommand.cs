using UnityEngine;

namespace Units
{
    public class MoveToPointCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 0.1f;
        private readonly Vector3 _destination;

        public MoveToPointCommand(Vector3 destination)
        {
            _destination = destination;
        }

        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            NavigationAgent.destination = _destination;
        }

        public override void UpdateCommandState()
        {
            if (UnitHasReachedCurrentDestination(ReachedDestinationThreshold))
            {
                CompleteCommand();
            }
        }
    }
}