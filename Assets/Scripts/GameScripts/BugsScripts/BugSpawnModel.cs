using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameScripts.BugsScripts
{
    public class BugSpawnModel
    {
        private GameSystemsHandler _context;

        public void Initialize(GameSystemsHandler context)
        {
            _context = context;
        }

        public void CreateBug(string addressKey, Vector3 position, BuildingModel target, BuildingColors color, float travelDistance, float speed)
        {
            Addressables.InstantiateAsync(addressKey, position, Quaternion.identity).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SetupNewBug(handle.Result, target, color, travelDistance, speed);
                }
                else
                {
                    Debug.LogError($"Can't spawn bug: {addressKey}");
                }
            };
        }

        private void SetupNewBug(GameObject go, BuildingModel target, BuildingColors color, float travelDistance, float speed)
        {
            var view = go.GetComponent<BugView>();
            var model = new BugModel();
            var system = new BugSystem(model, view);

            model.Initialize(system, view, target, color, _context, travelDistance, speed);
            
            system.InitSystem(_context);
            _context.AddNewSystem(system);
        }

        public void NotifyBugDied(BugModel bug)
        {
            
        }

        public void CreateDeathParticle(string pendingBugAddress, Vector3 hitDataPoint)
        {
            
        }
    }
}