public abstract class UnitAction
{
    protected UnitController Unit;
    
    public virtual void BeginAction(UnitController unitController)
    {
        Unit = unitController;
    }

    public virtual void UpdateAction()
    {
        
    }

    public virtual void CancelledAction()
    {
        
    }

    public virtual void CompleteAction()
    {
        Unit.CompleteAction();
    }
}