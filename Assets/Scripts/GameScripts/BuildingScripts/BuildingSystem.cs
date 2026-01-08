using System.Linq;
using GameScripts.BugsScripts;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingSystem : IGameSystem
    {
        public BuildingModel Model;
        public BuildingView View;

        public BuildingSystem(BuildingModel model, BuildingView view, BuildingData data)
        {
            Model = model;
            View = view;
            
            Model.InitializeBuilding(this, view, data);
        }

        public void InitSystem(GameSystemsHandler context)
        {
            Model.SetContext(context);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }

        
    }
}