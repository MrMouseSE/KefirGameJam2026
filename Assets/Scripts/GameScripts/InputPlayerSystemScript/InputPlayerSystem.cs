namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerSystem : IGameSystem
    {
        public InputPlayerModel Model;
        
        public void InitSystem(GameSystemsHandler context)
        {
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }
    }
}