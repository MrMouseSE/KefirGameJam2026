using System.Collections.Generic;
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

        public void CreateBug(string addressKey, Vector3 position, BuildingModel target, BuildingColors color, float travelDistance, float speed, List<FloorView> floorsToEat)
        {
            Addressables.InstantiateAsync(addressKey, position, Quaternion.identity).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SetupNewBug(handle.Result, target, color, travelDistance, speed, floorsToEat);
                }
                else
                {
                    Debug.LogError($"Can't spawn bug: {addressKey}");
                }
            };
        }

        private void SetupNewBug(GameObject go, BuildingModel target, BuildingColors color, float travelDistance, float speed, List<FloorView> floorsToEat)
        {
            var view = go.GetComponent<BugView>();
            
            view.BugAnimationEvents.CurrentBugColor = color;
            var levelDesc = _context.CurrentLevelDescription;
            if (levelDesc != null)
            {
                var colorData = levelDesc.SpriteColorByBuildingColor.Find(x => x.BuildingColor == color);
                
                if (colorData != null)
                {
                    var renderers = view.GetComponentsInChildren<SpriteRenderer>();
                    
                    if (renderers.Length > 0)
                    {
                        var sharedMatInstance = renderers[0].material;
                        
                        sharedMatInstance.SetTexture("_TintGradient", colorData.SpriteColorValue);
                        
                        foreach (var r in renderers)
                        {
                            r.material = sharedMatInstance;
                        }
                        
                        view.BugMaterial = sharedMatInstance;
                    }
                }
            }
            
            
            var model = new BugModel();
            var system = new BugSystem(model, view);
            
            model.Initialize(system, view, target, color, _context, travelDistance, speed, floorsToEat);
            
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