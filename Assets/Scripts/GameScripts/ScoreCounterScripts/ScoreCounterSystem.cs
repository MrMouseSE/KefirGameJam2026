namespace GameScripts.ScoreCounterScripts
{
    public class ScoreCounterSystem : IGameSystem
    {
        public ScoreCounterModel Model;
        public ScoreCounterView View;

        public ScoreCounterSystem(ScoreCounterModel model, ScoreCounterView view)
        {
            View = view;
            Model = model;
            Model.InitModel(this, view);
        }

        public void InitSystem(GameSystemsHandler context)
        {
            Model.InitSystems(context);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }
    }
}