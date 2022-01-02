using System.Collections.Generic;
using Interactables;
using UnityEngine;

namespace In_Game_Resource_Storage
{
    public class ResourceStorageBuilding : MonoBehaviour
    {
        [SerializeField] private ResourceStorageManager resourceStorageManager;
        [SerializeField] private List<CollectableStorageSpot> storageSpots;

        public void DepositCollectable(CollectableObject collectableObject)
        {
            for (var i = 0; i < storageSpots.Count; i++)
            {
                if (!storageSpots[i].IsNotOccupied()) continue;
                storageSpots[i].StoreCollectable(collectableObject);
                return;
            }
        }
    }
}