using GameScripts.BuildingScripts;

namespace GameScripts.BuildingsPlacerSystemScripts
{
    public class BuildingsPlacerSystem : IGameSystem
    {
        public BuildingsPlacerModel Model;
        public BuildingsPlacerView View;

        public BuildingsPlacerSystem(BuildingsPlacerModel model, BuildingsPlacerView view)
        {
            Model = model;
            Model.InitializeModel(view, this);
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