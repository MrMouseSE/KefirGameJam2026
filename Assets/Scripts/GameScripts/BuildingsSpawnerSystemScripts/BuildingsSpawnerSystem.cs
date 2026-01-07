using GameScripts.Descriptions;

namespace GameScripts.BuildingsSpawnerSystemScripts
{
    public class BuildingsSpawnerSystem : IGameSystem
    {
        public BuildingsSpawnerModel Model;

        public BuildingsSpawnerSystem(BuildingsSpawnerModel model, LevelDescription levelDescription)
        {
            Model = model;
            Model.InitializeLevelBuildings(levelDescription, this);
        }

        public void InitSystem(GameSystemsHandler context)
        {
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }
    }
}