using System;
using System.Collections.Generic;
using GameScripts.BuildingScripts;

namespace GameScripts.BuildingsPlacerSystemScripts
{
    public class BuildingsPlacerModel
    {
        public BuildingsPlacerSystem System;
        public BuildingsPlacerView View;
        
        public List<BuildingSystem> Buildings;
        
        public Action OnAllBuildingsDestroyed;

        public void InitializeModel(BuildingsPlacerView view, BuildingsPlacerSystem system)
        {
            System = system;
            View = view;
            Buildings = new List<BuildingSystem>();
        }

        public void AddBuilding(BuildingSystem building)
        {
            Buildings.Add(building);
        }

        public void RemoveBuilding(BuildingSystem building)
        {
            Buildings.Remove(building);
            if (Buildings.Count > 0) return;
            OnAllBuildingsDestroyed?.Invoke();
        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            //TODO: update buildings positions
        }
    }
}