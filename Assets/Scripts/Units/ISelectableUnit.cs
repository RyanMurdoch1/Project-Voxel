using UnityEngine;

public interface ISelectableUnit
{
    void Select();
    void Unselect();
    void IssueDirection(Vector3 destination, bool addToQueue);
}