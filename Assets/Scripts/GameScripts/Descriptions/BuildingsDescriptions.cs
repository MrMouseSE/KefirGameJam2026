using System.Collections.Generic;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.Descriptions
{
    public class BuildingsDescriptions : ScriptableObject
    {
        public float BuildingChancePercent;
        public BuildingView BuildingPrefab;
        public List<FloorSpriteColorPair> BuildingSpriteColorPairs;
    }
}