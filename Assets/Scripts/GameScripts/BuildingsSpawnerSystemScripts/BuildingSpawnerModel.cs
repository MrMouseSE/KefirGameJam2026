using GameScripts.Descriptions;

namespace GameScripts.BuildingsSpawnerSystemScripts
{
    public class BuildingSpawnerModel
    {
        private int _buildingsSpawned = 0;
        private int _buildingsFinalCount = 0;

        public void InitializeLevelBuildings(LevelDescription levelDescription)
        {
            _buildingsFinalCount = levelDescription.GetBuildingsCount();
        }
        
        
    }
}