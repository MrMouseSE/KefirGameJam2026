namespace GameScripts
{
    public interface IGameSystem
    {
        public void InitSystem(GameSystemsHandler context);
        public void UpdateSystem(float deltaTime, GameSystemsHandler context);
    }
}