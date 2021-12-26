using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour, ISelectableUnit
{
    [SerializeField] private Material selectedMaterial, unselectedMaterial;
    private const float ReachedDestinationThreshold = 0.1f;
    private NavMeshAgent _unit;
    private Queue<Vector3> _destinationsQueue;
    private Vector3 _currentDestination;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _unit = GetComponent<NavMeshAgent>();
        _destinationsQueue = new Queue<Vector3>(5);
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void IssueDirection(Vector3 destination, bool addToQueue)
    {
        if (IsDestinationForQueue(destination, addToQueue))
        {
            _destinationsQueue.Enqueue(destination);
        }
        else
        {
            _destinationsQueue.Clear();
            _currentDestination = destination;
            UpdateUnitDestination();
        }
    }

    public void Select() => _meshRenderer.material = selectedMaterial;

    public void Unselect() => _meshRenderer.material = unselectedMaterial;

    private void Update()
    {
        CheckDestinationQueue();
    }

    private bool IsDestinationForQueue(Vector3 destination, bool addToQueue)
    {
        return addToQueue && _destinationsQueue.Count != 0 || addToQueue && destination != _currentDestination;
    }
    
    private void UpdateUnitDestination() => _unit.destination = _currentDestination;

    private void CheckDestinationQueue()
    {
        if (!UnitHasReachedCurrentDestination() || !UnitHasQueuedDestination()) return;
        _currentDestination = _destinationsQueue.Dequeue();
        UpdateUnitDestination();
    }

    private bool UnitHasQueuedDestination() => _destinationsQueue.Count != 0;

    private bool UnitHasReachedCurrentDestination() => _unit.remainingDistance < ReachedDestinationThreshold;
}
