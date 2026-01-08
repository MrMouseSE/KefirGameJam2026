using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameScripts.BugsScripts
{
    public class BugSystem : IGameSystem
    {
        public BugModel Model;
        public BugView View;
        
        private GameSystemsHandler _context;
        
        public BugSystem(BugModel model, BugView view)
        {
            Model = model;
            View = view;
        }

        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;
        }
        
        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime);
        }
    }
}