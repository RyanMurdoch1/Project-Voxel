using System.Collections.Generic;
using Interactables;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    [RequireComponent(typeof(ISelectionVisualizer))]
    public class UnitController : MonoBehaviour, ISelectableUnit
    {
        [SerializeField] private UnitInformation unitInformation;
        [SerializeField] private Transform unitHand;

        private CollectableObject _currentCollectable;
        private NavMeshAgent _navigationAgent;
        private ISelectionVisualizer _selectionVisualizer;
        private bool _hasUncompletedCommand;
        private UnitCommand _currentCommand;
        private Queue<UnitCommand> _commandQueue;

        private void Awake()
        {
            _selectionVisualizer = GetComponent<ISelectionVisualizer>();
            _navigationAgent = GetComponent<NavMeshAgent>();
            _navigationAgent.speed = unitInformation.unitSpeed;
            _commandQueue = new Queue<UnitCommand>();
        }

        public NavMeshAgent GetUnitNavigationAgent() => _navigationAgent;

        public void ReceiveCollectable(CollectableObject collectable)
        {
            _currentCollectable = collectable;
            Transform collectableTransform;
            (collectableTransform = _currentCollectable.transform).SetParent(unitHand);
            collectableTransform.localPosition = Vector3.zero;
        }

        public void DropCollectable()
        {
            _currentCollectable.CancelInteraction(this);
            _currentCollectable = null;
        }

        public void DistributeCollectable()
        {
            Destroy(_currentCollectable.gameObject);
        }
        
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

        public void IssueCommandOverride(UnitCommand action)
        {
            StartNewCommand(action);
        }

        public void CompleteCommand()
        {
            if (UnitHasQueuedCommand())
            {
                StartNewCommand(_commandQueue.Dequeue());
                return;
            }

            SetUnitIdle();
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
        
        private void SetUnitIdle()
        {
            _navigationAgent.destination = transform.position;
            _currentCommand = null;
            _hasUncompletedCommand = false;
        }

        private bool IsCommandForQueue(bool addToQueue) => addToQueue && _hasUncompletedCommand;

        private bool UnitHasQueuedCommand() => _commandQueue.Count != 0;
    }
}