using Units;
using UnityEngine;

namespace Interactables
{
    public interface IInteractableObject
    {
        public void BeginInteraction(UnitController unit);
        public void CancelInteraction(UnitController unit);
        public Vector3 GetWorldLocation(); 
        public bool IsValidInteraction();
    }
}