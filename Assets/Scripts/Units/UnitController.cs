using System.Collections.Generic;
using Helpers;
using In_Game_Resource_Storage;
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

        public void DistributeCollectable(ResourceStorageBuilding storageBuilding)
        {
            storageBuilding.DepositCollectable(_currentCollectable);
        }
        
        public void IssueCommand(UnitCommand command, bool addToQueue)
        {
            if (IsCommandForQueue(addToQueue))
            {
                _commandQueue.Enqueue(command);
                return;
            }

            _commandQueue.Clear();
            StartNewCommand(command);
        }

        public Vector3 GetUnitPosition()
        {
            return gameObject.transform.position;
        }

        public void IssueCommandOverride(UnitCommand command)
        {
            StartNewCommand(command);
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
    
        private void StartNewCommand(UnitCommand command)
        {
            _hasUncompletedCommand = true;
            _currentCommand?.CancelCommand();
            _currentCommand = command;
            _currentCommand.BeginCommand(this);
        }

        private void Update()
        {
            if (_hasUncompletedCommand && UpdateHelper.IsUpdatingFrame())
            {
                _currentCommand.UpdateCommandState();
            }
        }
        
        private void SetUnitIdle()
        {
            _currentCommand = null;
            _hasUncompletedCommand = false;
        }

        private bool IsCommandForQueue(bool addToQueue) => addToQueue && _hasUncompletedCommand;

        private bool UnitHasQueuedCommand() => _commandQueue.Count != 0;
    }
}