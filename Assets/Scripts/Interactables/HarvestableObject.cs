using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Interactables
{
    public class HarvestableObject : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private HarvestableData harvestableData;
        private readonly List<ISelectableUnit> _harvestingUnits = new List<ISelectableUnit>();
        private readonly List<Vector3> _harvestingPositions = new List<Vector3>();
        private float _harvestRemainingSeconds;
        private bool _isBeingHarvested;
        private int _harvestingUnitCount, _positionIndex;
        private Vector3 _objectPosition;

        private void Awake()
        {
            _objectPosition = transform.position;
            _harvestRemainingSeconds = harvestableData.harvestingTimeInSeconds;
            CalculateHarvestingPositions();
        }

        private void CalculateHarvestingPositions()
        {
            for (var i = 0; i < harvestableData.maxHarvestingUnits; i++)
            {
                float radius = harvestableData.maxHarvestingUnits;
                var angle = i * Mathf.PI * 2f / radius;
                var position = _objectPosition + new Vector3(Mathf.Cos(angle) * radius, -2, Mathf.Sin(angle) * radius);
                _harvestingPositions.Add(position);
            }
        }

        public void BeginInteraction(ISelectableUnit unit)
        {
            _harvestingUnits.Add(unit);
            _harvestingUnitCount = _harvestingUnits.Count;
            if (_isBeingHarvested) return;
            _isBeingHarvested = true;
            StartCoroutine(HarvestObjectRoutine());
        }
        
        public void CancelInteraction(ISelectableUnit unit)
        {
            _harvestingUnits.Remove(unit);
            _harvestingUnitCount = _harvestingUnits.Count;
            if (_harvestingUnitCount != 0) return;
            _isBeingHarvested = false;
            StopAllCoroutines();
        }

        public Vector3 GetUnitDestination()
        {
            _positionIndex++;
            if (_positionIndex >= _harvestingPositions.Count)
            {
                _positionIndex = 0;
            }
            return _harvestingPositions[_positionIndex];
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

                spawnedCollectable.gameObject.transform.position = _objectPosition;
            }

            gameObject.SetActive(false);
        }
    }
}