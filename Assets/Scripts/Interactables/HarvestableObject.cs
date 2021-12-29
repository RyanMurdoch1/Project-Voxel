using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Interactables
{
    public class HarvestableObject : MonoBehaviour, IInteractableObject
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

        public void BeginInteraction(UnitController unit)
        {
            _harvestingUnits.Add(unit);
            if (!_isBeingHarvested)
            {
                _isBeingHarvested = true;
                StartCoroutine(HarvestObjectRoutine());
            }

            _harvestingUnitCount = _harvestingUnits.Count;
        }

        public void CancelInteraction(UnitController unit)
        {
            _harvestingUnits.Remove(unit);
            _harvestingUnitCount = _harvestingUnits.Count;
            if (_harvestingUnitCount != 0) return;
            _isBeingHarvested = false;
            StopAllCoroutines();
        }

        public Vector3 GetWorldLocation()
        {
            return gameObject.transform.position;
        }

        public bool IsValidInteraction()
        {
            return _harvestRemainingSeconds > 0;
        }

        private IEnumerator HarvestObjectRoutine()
        {
            while (_harvestRemainingSeconds > 0 && _isBeingHarvested)
            {
                _harvestRemainingSeconds -= Time.deltaTime * _harvestingUnitCount;
                yield return null;
            }
        
            CompleteHarvest();
        }

        private void CompleteHarvest()
        {
            for (var i = 0; i < _harvestingUnitCount; i++)
            {
                _harvestingUnits[i].CompleteCommand();
            }

            for (var i = 0; i < harvestableData.harvestableContents.Count; i++)
            {
                var spawnedCollectable = Instantiate(harvestableData.harvestableContents[i]);
                if (i < _harvestingUnitCount)
                {
                    spawnedCollectable.BeginInteraction(_harvestingUnits[i]);
                    continue;
                }

                spawnedCollectable.gameObject.transform.position = gameObject.transform.position;
            }

            gameObject.SetActive(false);
        }
    }
}