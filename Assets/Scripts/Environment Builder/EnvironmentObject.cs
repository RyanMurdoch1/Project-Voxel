using Sirenix.OdinInspector;
using UnityEngine;

namespace Environment_Builder
{
    [CreateAssetMenu(fileName = "Environment Object")]
    public class EnvironmentObject : ScriptableObject
    {
        [PreviewField, Required, AssetsOnly]
        public GameObject objectPrefab;
        public float placementYOffset;
        public Material placementMaterial;
        public EnvironmentObjectType objectType;
    }
}
