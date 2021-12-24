using UnityEngine;

public interface ICommandable
{
    void Select();
    void Unselect();
    void IssueDirection(Vector3 destination, bool addToQueue);
}