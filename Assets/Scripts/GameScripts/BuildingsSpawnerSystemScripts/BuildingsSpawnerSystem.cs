namespace GameScripts.BuildingsSpawnerSystemScripts
{
    public class BuildingsSpawnerSystem : IGameSystem
    {
        public BuildingSpawnerModel Model;

        public BuildingsSpawnerSystem(BuildingSpawnerModel model)
        {
            Model = model;
        }

        public void InitSystem(GameSystemsHandler context)
        {
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
        }
    }
}