using UnityEngine;
using GameScripts.BuildingScripts;

namespace GameScripts.CannonScripts
{
    public class CannonSystem : IGameSystem
    {
        public CannonModel Model;
        public CannonView View;
        private GameSystemsHandler _context;
        
        public CannonSystem(CannonModel model, CannonView view)
        {
            Model = model;
            View = view;
        }

        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;
            Model.Initialize(context, View);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }
    }
}