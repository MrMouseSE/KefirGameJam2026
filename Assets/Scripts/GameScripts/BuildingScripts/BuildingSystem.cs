namespace GameScripts.BuildingScripts
{
    public class BuildingSystem : IGameSystem
    {
        public BuildingModel Model;
        public BuildingView View;

        public BuildingSystem(BuildingModel model, BuildingView view)
        {
            Model = model;
            Model.InitializeBuilding(this, view);
            View = view;
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