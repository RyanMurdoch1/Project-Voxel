using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public abstract class UnitCommand
    {
        protected ISelectableUnit Unit;
        protected NavMeshAgent NavigationAgent;
    
        public virtual void BeginCommand(ISelectableUnit unitController)
        {
            Unit = unitController;
            NavigationAgent = Unit.GetUnitNavigationAgent();
        }

        public virtual void UpdateCommandState()
        {
        
        }

        public virtual void CancelCommand()
        {
        
        }

        protected void CompleteCommand()
        {
            Unit.CompleteCommand();
        }
        
        protected bool UnitHasReachedCurrentDestination(float reachedDestinationThreshold, Vector3 targetDestination)
        {
            return Vector3.Distance(Unit.GetUnitPosition(), targetDestination) < reachedDestinationThreshold;
        }
    }
}