using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableObject : MonoBehaviour
{
    [SerializeField] private HarvestableData harvestableData;
    private List<UnitController> _harvestingUnits;
    private float _harvestRemainingSeconds;
    private bool _isBeingHarvested;
    private int _harvestingUnitCount;

    private void Awake()
    {
        _harvestingUnits = new List<UnitController>();
        _harvestRemainingSeconds = harvestableData.harvestingTimeInSeconds;
    }

    public void BeginHarvesting(UnitController unit)
    {
        _harvestingUnits.Add(unit);
        if (!_isBeingHarvested)
        {
            _isBeingHarvested = true;
            StartCoroutine(HarvestObjectRoutine());
        }

        _harvestingUnitCount = _harvestingUnits.Count;
    }

    public void CancelHarvesting(UnitController unit)
    {
        _harvestingUnits.Remove(unit);
        _harvestingUnitCount = _harvestingUnits.Count;
        if (_harvestingUnitCount != 0) return;
        _isBeingHarvested = false;
        StopCoroutine(HarvestObjectRoutine());
    }

    public bool HasBeenHarvested()
    {
        return _harvestRemainingSeconds <= 0;
    }

    private IEnumerator HarvestObjectRoutine()
    {
        while (_harvestRemainingSeconds > 0)
        {
            _harvestRemainingSeconds -= Time.deltaTime;
            yield return null;
        }
        
        CompleteHarvest();
    }

    private void CompleteHarvest()
    {
        
    }
}