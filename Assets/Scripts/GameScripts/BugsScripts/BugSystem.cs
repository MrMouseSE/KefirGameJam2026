namespace GameScripts.BugsScripts
{
    public class BugSystem : IGameSystem
    {
        public BugModel Model;
        public BugView View;

        public BugSystem(BugModel model, BugView view)
        {
            Model = model;
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