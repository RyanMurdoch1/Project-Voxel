using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour, ISelectableUnit
{
    [SerializeField] private Material selectedMaterial, unselectedMaterial;
    
    private NavMeshAgent _navigationAgent;
    private bool _hasAction;
    private UnitAction _currentAction;
    private Queue<UnitAction> _actionQueue;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _navigationAgent = GetComponent<NavMeshAgent>();
        _actionQueue = new Queue<UnitAction>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public NavMeshAgent GetUnitNavigationAgent()
    {
        return _navigationAgent;
    }
    
    public void IssueAction(UnitAction action, bool addToQueue)
    {
        if (IsActionForQueue(addToQueue))
        {
            _actionQueue.Enqueue(action);
            return;
        }

        _actionQueue.Clear();
        StartNewAction(action);
    }

    public void CompleteAction()
    {
        if (UnitHasQueuedAction())
        {
            StartNewAction(_actionQueue.Dequeue());
            return;
        }

        _hasAction = false;
    }
    
    public void Select() => _meshRenderer.material = selectedMaterial;

    public void Unselect() => _meshRenderer.material = unselectedMaterial;
    
    private void StartNewAction(UnitAction action)
    {
        _hasAction = true;
        _currentAction = action;
        _currentAction.BeginAction(this);
    }

    private void Update()
    {
        if (_hasAction)
        {
            _currentAction.UpdateAction();
        }
    }

    private bool IsActionForQueue(bool addToQueue) => addToQueue && _hasAction;

    private bool UnitHasQueuedAction() => _actionQueue.Count != 0;
}