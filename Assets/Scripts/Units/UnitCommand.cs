namespace Units
{
    public abstract class UnitCommand
    {
        protected UnitController Unit;
    
        public virtual void BeginCommand(UnitController unitController)
        {
            Unit = unitController;
        }

        public virtual void UpdateCommandState()
        {
        
        }

        public virtual void CancelCommand()
        {
        
        }

        protected virtual void CompleteCommand()
        {
            Unit.CompleteCommand();
        }
    }
}