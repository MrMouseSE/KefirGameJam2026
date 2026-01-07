using System.Collections.Generic;
using GameScripts.BuildingScripts;
using GameScripts.Descriptions;
using UnityEngine;

namespace GameScripts.BuildingFactory
{
    public static class BuildingStaticFactory
    {
        private static LevelDescription _levelDescription;

        public static void SetLevelDescription(LevelDescription levelDescription)
        {
            _levelDescription = levelDescription;
        }
        
        public static BuildingSystem CreateBuildingSystem(float weight)
        {
            var level = _levelDescription.GetBuildingDescriptionByChance(weight);
            BuildingView buildingView = Object.Instantiate(level.BuildingPrefab);
            buildingView.Floors = new List<FloorView>();
            for (int i = 0; i < level.BuildingSpriteColorPairs.Count; i++)
            {
                var floorData = level.BuildingSpriteColorPairs[i];
                var floorView = Object.Instantiate(floorData.FloorPrefab, buildingView.FloorsTransform);
                floorView.FloorTransform.localPosition = i * Vector3.up * floorData.FloorHeight;
                buildingView.Floors.Add(floorView);
            }
            BuildingModel buildingModel = new BuildingModel();
            BuildingSystem buildingSystem = new BuildingSystem(buildingModel, buildingView);
            return buildingSystem;
        }
    }
}