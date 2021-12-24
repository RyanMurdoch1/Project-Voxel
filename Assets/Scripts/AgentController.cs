using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour, ICommandable
{
    [SerializeField] private Material selectedMaterial, unselectedMaterial;
    private const float ReachedDestinationThreshold = 0.1f;
    private NavMeshAgent _agent;
    private Queue<Vector3> _destinationsQueue;
    private Vector3 _currentDestination;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
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
        }
        
        UpdateAgentPosition();
    }

    private void Update()
    {
        CheckDestinationQueue();
    }

    private bool IsDestinationForQueue(Vector3 destination, bool addToQueue)
    {
        return addToQueue && _destinationsQueue.Count != 0 || addToQueue && destination != _currentDestination;
    }
    
    private void UpdateAgentPosition()
    {
        if (_currentDestination != Vector3.zero)
        {
            _agent.destination = _currentDestination;
        }
    }

    private void CheckDestinationQueue()
    {
        if (AgentHasQueuedDestination())
        {
            _agent.destination = _destinationsQueue.Dequeue();
        }
    }

    private bool AgentHasQueuedDestination()
    {
        return _agent.remainingDistance < ReachedDestinationThreshold && _destinationsQueue.Count != 0;
    }

    public void Select() => _meshRenderer.material = selectedMaterial;

    public void Unselect() => _meshRenderer.material = unselectedMaterial;
}
