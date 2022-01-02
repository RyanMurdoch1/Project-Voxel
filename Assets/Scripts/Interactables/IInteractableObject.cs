using Units;
using UnityEngine;

namespace Interactables
{
    public interface IInteractableObject
    {
        public void BeginInteraction(ISelectableUnit unit);
        public void CancelInteraction(ISelectableUnit unit);
        public Vector3 GetUnitDestination(); 
        public bool IsValidInteraction();
    }
}