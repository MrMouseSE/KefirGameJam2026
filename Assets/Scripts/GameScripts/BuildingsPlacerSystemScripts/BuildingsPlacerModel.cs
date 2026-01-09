using System;
using System.Collections.Generic;
using GameScripts.BuildingScripts;
using GameScripts.Descriptions;

namespace GameScripts.BuildingsPlacerSystemScripts
{
    public class BuildingsPlacerModel
    {
        public BuildingsPlacerSystem System;
        public BuildingsPlacerView View;
        
        public List<BuildingSystem> Buildings;
        
        public Action OnAllBuildingsDestroyed;
        public bool IsAllBuildingsDestroyed;

        private int _buildingsWidth;

        public void InitializeModel(BuildingsPlacerView view, BuildingsPlacerSystem system, LevelDescription levelDescription)
        {
            System = system;
            View = view;
            Buildings = new List<BuildingSystem>();
            _buildingsWidth = levelDescription.BuildingsWidthCount;
        }

        public void AddBuilding(BuildingSystem building)
        {
            Buildings.Add(building);
            IsAllBuildingsDestroyed = false;
        }

        public void RemoveBuilding(BuildingSystem building)
        {
            Buildings.Remove(building);
            if (Buildings.Count > 0) return;
            IsAllBuildingsDestroyed = true;
            OnAllBuildingsDestroyed?.Invoke();
        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (IsAllBuildingsDestroyed)
            {
                context.CompleteCurrentLevel();
                return;
            }
            bool isFirstLineNotEmpty = false;
            for (int i = 0; i < View.BuildingsFirstLinePlaces.Length; i++)
            {
                isFirstLineNotEmpty |= View.BuildingsFirstLinePlaces[i].IsHolderOccupied;
            }
            if (isFirstLineNotEmpty) return;
            
            
            for (int i = 0; i < _buildingsWidth; i++)
            {
                if (i > Buildings.Count - 1)
                {
                    View.BuildingsFirstLinePlaces[i].IsHolderOccupied = false;
                    return;
                }
                View.BuildingsFirstLinePlaces[i].BuildingView = Buildings[i].View;
                Buildings[i].View.BuildingTransform.position = View.BuildingsFirstLinePlaces[i].BuildingPlace.position;
                SetCollider(Buildings[i].View.Floors, true);
                View.BuildingsFirstLinePlaces[i].IsHolderOccupied = true;
            }

            for (int i = _buildingsWidth; i < _buildingsWidth * 2; i++)
            {
                if (i > Buildings.Count - 1)
                {
                    View.BuildingsSecondLinePlaces[i-_buildingsWidth].IsHolderOccupied = false;
                    continue;
                }
                View.BuildingsSecondLinePlaces[i-_buildingsWidth].BuildingView = Buildings[i].View;
                Buildings[i].View.BuildingTransform.position = View.BuildingsSecondLinePlaces[i-_buildingsWidth].BuildingPlace.position;
                SetCollider(Buildings[i].View.Floors, false);
                View.BuildingsSecondLinePlaces[i-_buildingsWidth].IsHolderOccupied = true;
            }
        }

        private void SetCollider(List<FloorView> floorViews, bool isEnabled)
        {
            foreach (var floor in floorViews)
            {
                floor.FloorCollider.enabled = isEnabled;
            }
        }
    }
}