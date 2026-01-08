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

        public void CreateBug(string addressKey, Vector3 position, BuildingModel target, BuildingColors color)
        {
            Addressables.InstantiateAsync(addressKey, position, Quaternion.identity).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SetupNewBug(handle.Result, target, color);
                }
                else
                {
                    Debug.LogError($"Can't spawn bug: {addressKey}");
                }
            };
        }

        private void SetupNewBug(GameObject go, BuildingModel target, BuildingColors color)
        {
            var view = go.GetComponent<BugView>();
            var model = new BugModel();
            var system = new BugSystem(model, view);

            model.Initialize(system, view, target, color, _context);
            
            system.InitSystem(_context);
            _context.AddNewSystem(system);
        }

        public void NotifyBugDied(BugModel bug)
        {
            
        }
    }
}