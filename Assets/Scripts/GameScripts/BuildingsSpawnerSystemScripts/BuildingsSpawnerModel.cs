using GameScripts.BuildingFactory;
using GameScripts.BuildingScripts;
using GameScripts.Descriptions;
using UnityEngine;

namespace GameScripts.BuildingsSpawnerSystemScripts
{
    public class BuildingsSpawnerModel
    {
        public BuildingsSpawnerSystem System;
        
        private bool _levelBuildingsWasInitialized;
        
        private int _buildingsSpawned = 0;
        private int _buildingsFinalCount = 0;

        public void InitializeLevelBuildings(LevelDescription levelDescription, BuildingsSpawnerSystem system)
        {
            System = system;
            _levelBuildingsWasInitialized = false;
            _buildingsFinalCount = levelDescription.GetBuildingsCount();
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (_levelBuildingsWasInitialized)
            {
                context.AddSystemToDelete(System);
            }
            
            if (_buildingsSpawned >= _buildingsFinalCount)
            {
                _levelBuildingsWasInitialized = true;
                return;
            }

            _buildingsSpawned++;
            BuildingSystem buildingSystem = BuildingStaticFactory.CreateBuildingSystem(Random.value);
            context.AddBuildingSystem(buildingSystem, _buildingsSpawned == _buildingsFinalCount);
        }
    }
}