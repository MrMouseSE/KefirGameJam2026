using GameScripts.Descriptions;

namespace GameScripts.BuildingsPlacerSystemScripts
{
    public class BuildingsPlacerSystem : IGameSystem
    {
        public BuildingsPlacerModel Model;
        public BuildingsPlacerView View;

        public BuildingsPlacerSystem(BuildingsPlacerModel model, BuildingsPlacerView view, LevelDescription levelDescription)
        {
            Model = model;
            Model.InitializeModel(view, this, levelDescription);
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