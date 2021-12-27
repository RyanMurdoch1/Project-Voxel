using System.Collections.Generic;
using UnityEngine;

namespace In_Game_Resource_Storage
{
    public class ResourceStorageManager : MonoBehaviour
    {
        [SerializeField] private List<ResourceStorageBuilding> resourceStorageBuildings;
        [SerializeField] private List<ResourceStorageBuilding> availableResourceStorageBuildings;

        private void Awake()
        {
            resourceStorageBuildings = new List<ResourceStorageBuilding>();
        }

        private bool HasAvailableStorageBuildings()
        {
            return availableResourceStorageBuildings.Count != 0;
        }

        public void SetStorageBuildingAvailable(ResourceStorageBuilding building, bool isAvailable)
        {
            if (isAvailable)
            {
                availableResourceStorageBuildings.Add(building);
            }
            else
            {
                availableResourceStorageBuildings.Remove(building);
            }
        }

        public bool FindAndReturnClosestStorageBuilding(Vector3 unitPosition, out ResourceStorageBuilding closestStorageBuilding)
        {
            if (!HasAvailableStorageBuildings())
            {
                closestStorageBuilding = null;
                return false;
            }
        
            var closestStorage = resourceStorageBuildings[0];
            for (var i = 0; i < resourceStorageBuildings.Count; i++)
            {
                if (BuildingAtIndexIsCloserThanCurrentClosestBuilding(unitPosition, i, closestStorage))
                {
                    closestStorage = resourceStorageBuildings[i];
                }
            }

            closestStorageBuilding = closestStorage;
            return true;
        }

        private bool BuildingAtIndexIsCloserThanCurrentClosestBuilding(Vector3 unitPosition, int i, Component closestStorage)
        {
            return Vector3.Distance(resourceStorageBuildings[i].transform.position, unitPosition) < Vector3.Distance(closestStorage.transform.position, unitPosition);
        }
    }
}