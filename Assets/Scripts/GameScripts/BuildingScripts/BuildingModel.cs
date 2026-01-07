using System.Collections.Generic;

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
            if (!context.IsBugSpawned) return;
            if (context.CurrentBug != null && context.CurrentBug.TargetBuilding != null)
            {
                if (context.CurrentBug.TargetBuilding.View != View) return;
            }
        }
    }
}