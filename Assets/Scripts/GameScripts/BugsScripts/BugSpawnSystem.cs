using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameScripts.BugsScripts
{
    public class BugSpawnSystem : IGameSystem
    {
        public BugSpawnModel Model;

        public BugSpawnSystem(BugSpawnModel model)
        {
            Model = model;
        }
        
        public void InitSystem(GameSystemsHandler context)
        {
            Model.Initialize(context);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            
        }
    }
}