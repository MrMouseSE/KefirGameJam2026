namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerSystem : IGameSystem
    {
        public InputPlayerModel Model;

        public InputPlayerSystem(InputPlayerModel model)
        {
            Model = model;
            Model.InitializeModel(this);
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