using In_Game_Resource_Storage;
using Interactables;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public interface ISelectableUnit
    {
        void Select();
        void Unselect();
        void IssueCommand(UnitCommand command, bool addToQueue);
        void IssueCommandOverride(UnitCommand command);
        void CompleteCommand();
        void ReceiveCollectable(CollectableObject collectable);
        void DistributeCollectable(ResourceStorageBuilding targetStorageBuilding);
        void DropCollectable();
        NavMeshAgent GetUnitNavigationAgent();
        Vector3 GetUnitPosition();
    }
}