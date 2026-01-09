using System.Collections.Generic;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.Descriptions
{
    [CreateAssetMenu(menuName = "Create BuildingsDescriptions", fileName = "BuildingsDescriptions", order = 0)]
    public class BuildingsDescriptions : ScriptableObject
    {
        public float BuildingChancePercent;
        public BuildingView BuildingPrefab;
        public int BuildingDestroyValue;
        public int FloorDestroyValue;
        public List<FloorSpriteColorPair> BuildingSpriteColorPairs;
    }
}