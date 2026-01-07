namespace GameScripts.BuildingScripts
{
    public class BuildingModel
    {
        public BuildingSystem System;
        public BuildingView View;

        public void InitializeBuilding(BuildingSystem system, BuildingView view)
        {
            System = system;
            View = view;
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (context.CurrentBug.TargetBuilding.View != View) return;
            
        }
    }
}