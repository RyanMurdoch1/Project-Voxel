using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    [CreateAssetMenu(menuName = "Create HarvestableData", fileName = "HarvestableData", order = 0)]
    public class HarvestableData : ScriptableObject
    {
        public float harvestingTimeInSeconds;
        public List<CollectableObject> harvestableContents;
    }
}