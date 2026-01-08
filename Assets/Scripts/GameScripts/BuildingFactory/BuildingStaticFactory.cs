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
            BuildingView buildingView = Object.Instantiate(level.BuildingPrefab, Vector3.right*100f, Quaternion.identity);
            buildingView.Floors = new List<FloorView>();
            BuildingData buildingData = new BuildingData();
            buildingData.FloorsData = new List<FloorData>();
            for (int i = 0; i < level.BuildingSpriteColorPairs.Count; i++)
            {
                var floorPair = level.BuildingSpriteColorPairs[i];
                var floorView = Object.Instantiate(floorPair.FloorPrefab, buildingView.FloorsTransform);
                floorView.FloorTransform.localPosition = Vector3.up * (i * floorPair.FloorHeight);
                buildingView.Floors.Add(floorView);
                FloorData floorData = new FloorData();
                floorData.FloorColor = level.BuildingSpriteColorPairs[i].BuildingColors[Random.Range(0,level.BuildingSpriteColorPairs[i].BuildingColors.Count)];
                floorView.FloorRenderer.sprite = level.BuildingSpriteColorPairs[i].FloorSprite;
                floorView.FloorRenderer.color = _levelDescription.SpriteColorByBuildingColor.Find
                    (x => x.BuildingColor == floorData.FloorColor).SpriteColorValue;
                buildingData.FloorsData.Add(floorData);
            }
            BuildingModel buildingModel = new BuildingModel();
            buildingModel.Data = buildingData;
            BuildingSystem buildingSystem = new BuildingSystem(buildingModel, buildingView);
            return buildingSystem;
        }
    }
}