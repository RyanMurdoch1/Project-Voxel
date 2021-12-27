using UnityEngine;
using UnityEngine.AI;

public class MoveToPointAction : UnitAction
{
    private const float ReachedDestinationThreshold = 0.1f;

    private readonly Vector3 _destination;
    private NavMeshAgent _navigationAgent;

    public MoveToPointAction(Vector3 destination)
    {
        _destination = destination;
    }

    public override void BeginAction(UnitController unitController)
    {
        base.BeginAction(unitController);
        _navigationAgent = Unit.GetUnitNavigationAgent();
        _navigationAgent.destination = _destination;
    }

    public override void UpdateAction()
    {
        if (UnitHasReachedCurrentDestination())
        {
            CompleteAction();
        }
    }
    
    private bool UnitHasReachedCurrentDestination() => _navigationAgent.remainingDistance < ReachedDestinationThreshold;
}