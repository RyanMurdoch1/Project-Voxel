using UnityEngine;

namespace Units
{
    [CreateAssetMenu(menuName = "Create UnitInformation", fileName = "UnitInformation", order = 0)]
    public class UnitInformation : ScriptableObject
    {
        public float unitSpeed;
        public int unitResourceCapacity;
    }
}