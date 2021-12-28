using UnityEngine.AI;

namespace Units
{
    public abstract class UnitCommand
    {
        protected UnitController Unit;
        protected NavMeshAgent NavigationAgent;
    
        public virtual void BeginCommand(UnitController unitController)
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
        
        protected bool UnitHasReachedCurrentDestination(float reachedDestinationThreshold) => NavigationAgent.remainingDistance < reachedDestinationThreshold;
    }
}