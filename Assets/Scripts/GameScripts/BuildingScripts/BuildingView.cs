using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingView : MonoBehaviour
    {
        public GameObject BuildingPrefab;
        public Transform BuildingTransform;
        public Transform FloorsTransform;
        public List<FloorView> Floors;
    }
}
