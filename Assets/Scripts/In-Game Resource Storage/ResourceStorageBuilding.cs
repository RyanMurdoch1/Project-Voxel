using System.Collections.Generic;
using UnityEngine;

namespace In_Game_Resource_Storage
{
    public class ResourceStorageBuilding : MonoBehaviour
    {
        [SerializeField] private ResourceStorageManager resourceStorageManager;
        [SerializeField] private GameObject[] resourceObjs;
        public int capacityInUnits;
        public List<Harvestable> heldResources;

        private void Awake()
        {
            heldResources = new List<Harvestable>(capacityInUnits);
        }

        public void DepositResources(Harvestable resource, int units)
        {
            for (var i = 0; i < units; i++)
            {
                heldResources.Add(resource);
            }

            if (heldResources.Count >= capacityInUnits)
            {
                resourceStorageManager.SetStorageBuildingAvailable(this, false);
            }

            for (var i = 0; i < heldResources.Count; i++)
            {
                resourceObjs[i].SetActive(true);
            }
        }
    }
}