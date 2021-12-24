using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSelectionManager : MonoBehaviour
{
    [SerializeField] private AgentController agentController;
    private Camera _camera;
    private LayerMask _traversableLayer;

    private void Awake()
    {
        _camera = Camera.main;
        _traversableLayer = LayerMask.NameToLayer("Traversable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                agentController.AddAgentDestination(hit.point, Input.GetKey(KeyCode.LeftShift));
            }
        }
    }
}

// ICommandable