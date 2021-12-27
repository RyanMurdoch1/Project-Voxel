using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class MoveToPointCommand : UnitCommand
    {
        private const float ReachedDestinationThreshold = 0.1f;

        private readonly Vector3 _destination;
        private NavMeshAgent _navigationAgent;

        public MoveToPointCommand(Vector3 destination)
        {
            _destination = destination;
        }

        public override void BeginCommand(UnitController unitController)
        {
            base.BeginCommand(unitController);
            _navigationAgent = Unit.GetUnitNavigationAgent();
            _navigationAgent.destination = _destination;
        }

        public override void UpdateCommandState()
        {
            if (UnitHasReachedCurrentDestination())
            {
                CompleteCommand();
            }
        }
    
        private bool UnitHasReachedCurrentDestination() => _navigationAgent.remainingDistance < ReachedDestinationThreshold;
    }
}