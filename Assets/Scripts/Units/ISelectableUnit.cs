
public interface ISelectableUnit
{
    void Select();
    void Unselect();
    void IssueAction(UnitAction action, bool addToQueue);
}