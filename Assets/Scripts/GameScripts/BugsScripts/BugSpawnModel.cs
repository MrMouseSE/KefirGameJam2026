using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameScripts.BugsScripts
{
    public class BugSpawnModel
    {
        private const string DEFAULT_BUG_ADDRESS = "DefaultBug";

        public bool IsBugSpawned;
        public BugModel CurrentBug;
        private GameSystemsHandler _context;

        public void Initialize(GameSystemsHandler context)
        {
            _context = context;
        }

        public void CreateAndRegisterBug(string addressKey, Vector3 position, BuildingModel target, BuildingColors assignedColor)
        {
            var keyToLoad = string.IsNullOrEmpty(addressKey) ? DEFAULT_BUG_ADDRESS : addressKey;

            Addressables.InstantiateAsync(keyToLoad, position, Quaternion.identity).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var bugGo = handle.Result;

                    InitializeBugLogic(bugGo, target, assignedColor);
                }
                else
                {
                    Debug.LogError($"Failed to spawn bug from Addressables. Key: {keyToLoad}");
                }
            };
        }

        private void InitializeBugLogic(GameObject bugGo, BuildingModel target, BuildingColors assignedColor)
        {
            var bugView = bugGo.GetComponent<BugView>();

            if (_context.CurrentLevelDescription != null)
            {
                bugView.SetColor(assignedColor);
            }

            var bugModel = new BugModel();
            bugModel.TargetBuilding = target;
            bugModel.BugColor = assignedColor;

            var bugSystem = new BugSystem(bugModel, bugView);
            bugSystem.InitSystem(_context);

            _context.AddNewSystem(bugSystem);

            CurrentBug = bugModel;
            IsBugSpawned = true;
        }

        public void OnBugDied()
        {
            CurrentBug = null;
            IsBugSpawned = false;
        }
    }
}