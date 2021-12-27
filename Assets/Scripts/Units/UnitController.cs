using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(ISelectionVisualizer))]
    public class UnitController : MonoBehaviour, ISelectableUnit
    {
        private NavMeshAgent _navigationAgent;
        private ISelectionVisualizer _selectionVisualizer;
    
        private bool _hasUncompletedCommand;
        private UnitCommand _currentCommand;
        private Queue<UnitCommand> _commandQueue;

        private void Awake()
        {
            _selectionVisualizer = GetComponent<ISelectionVisualizer>();
            _navigationAgent = GetComponent<NavMeshAgent>();
            _commandQueue = new Queue<UnitCommand>();
        }

        public NavMeshAgent GetUnitNavigationAgent() => _navigationAgent;

        public void IssueCommand(UnitCommand action, bool addToQueue)
        {
            if (IsCommandForQueue(addToQueue))
            {
                _commandQueue.Enqueue(action);
                return;
            }

            _commandQueue.Clear();
            StartNewCommand(action);
        }

        public void CompleteCommand()
        {
            if (UnitHasQueuedCommand())
            {
                StartNewCommand(_commandQueue.Dequeue());
                return;
            }

            _hasUncompletedCommand = false;
        }
    
        public void Select() => _selectionVisualizer.OnSelect();

        public void Unselect() => _selectionVisualizer.OnDeselect();
    
        private void StartNewCommand(UnitCommand action)
        {
            _hasUncompletedCommand = true;
            _currentCommand?.CancelCommand();
            _currentCommand = action;
            _currentCommand.BeginCommand(this);
        }

        private void Update()
        {
            if (_hasUncompletedCommand)
            {
                _currentCommand.UpdateCommandState();
            }
        }

        private bool IsCommandForQueue(bool addToQueue) => addToQueue && _hasUncompletedCommand;

        private bool UnitHasQueuedCommand() => _commandQueue.Count != 0;
    }
}