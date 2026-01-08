using System.Collections.Generic;
using GameScripts.BugsScripts;

namespace GameScripts.BuildingScripts
{
    public class BuildingModel
    {
        public BuildingSystem System;
        public BuildingView View;
        public List<FloorData> CurrentFloors;
        
        public void InitializeBuilding(BuildingSystem system, BuildingView view, BuildingData data)
        {
            System = system;
            View = view;
            
            CurrentFloors = new List<FloorData>(data.FloorsData);
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            var system = context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;
            
            if (!system.Model.IsBugSpawned) return;
            if (system.Model.CurrentBug != null && system.Model.CurrentBug.TargetBuilding != null)
            {
                if (system.Model.CurrentBug.TargetBuilding.View != View) return;
            }
        }
    }
}