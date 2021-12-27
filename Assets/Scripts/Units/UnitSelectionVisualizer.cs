using UnityEngine;

namespace Units
{
    public class UnitSelectionVisualizer : MonoBehaviour, ISelectionVisualizer
    {
        [SerializeField] private Material selectedMaterial, unselectedMaterial;
        private MeshRenderer _meshRenderer;

        private void Awake() => _meshRenderer = GetComponent<MeshRenderer>();

        public void OnSelect() => _meshRenderer.material = selectedMaterial;

        public void OnDeselect() => _meshRenderer.material = unselectedMaterial;
    }
}