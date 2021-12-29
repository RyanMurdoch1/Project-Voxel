using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Units
{
    public class UnitSelectionVisualizer : MonoBehaviour, ISelectionVisualizer
    {
        [SerializeField] private DecalProjector selectionProjector;

        public void OnSelect() => selectionProjector.enabled = true;

        public void OnDeselect() => selectionProjector.enabled = false;
    }
}