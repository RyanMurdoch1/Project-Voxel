namespace Units
{
    public interface ISelectableUnit
    {
        void Select();
        void Unselect();
        void IssueCommand(UnitCommand action, bool addToQueue);
    }
}