using UnityEngine;

[CreateAssetMenu(menuName = "Create HarvestableData", fileName = "HarvestableData", order = 0)]
public class HarvestableData : ScriptableObject
{
    public float harvestingTimeInSeconds;
    public Harvestable harvestableType;
    public int totalHarvestableUnits;
}