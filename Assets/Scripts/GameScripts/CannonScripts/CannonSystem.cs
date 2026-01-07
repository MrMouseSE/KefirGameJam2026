using UnityEngine;
using GameScripts.BuildingScripts;

namespace GameScripts.CannonScripts
{
    public class CannonSystem : IGameSystem
    {
        public CannonModel Model;
        public CannonView View;
        private GameSystemsHandler _context;
        private const string DEFAULT_BUG_KEY = "DefaultBug";
        
        public CannonSystem(CannonModel model, CannonView view)
        {
            Model = model;
            View = view;
        }

        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;

            var levelData = context.CurrentLevelDescription;
            if (levelData != null) Model.LoadLevelAmmo(levelData, DEFAULT_BUG_KEY);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateCooldown(deltaTime);
        }

        public void Aim(Ray ray)
        {
            View.RotateCannon(Physics.Raycast(ray, out RaycastHit hit) ? hit.point : ray.GetPoint(100f));
        }
        
        public void TryFire(Ray ray)
        {
            if (!Model.IsReady)
            {
                Debug.LogError("[DEBUG] Перезарядка");
                return;
            }
            
            LogAmmoDebug();
            
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            
            var buildingView = hit.collider.GetComponentInParent<BuildingView>();
            if (buildingView == null) return;

            View.PlayFireEffects();
            
            var ammoData = Model.GetNextAmmo();
            
            var bugAddress = "DefaultBug";
            
            buildingView.ReceiveHit(hit, bugAddress, ammoData.Color);
            
            Model.SetCooldown(View.FireCooldown);
        }

        public void RemoveCurrentAmmo()
        {
            if (!Model.IsReady) return;
            
            LogAmmoDebug();
            
            Model.GetNextAmmo();
            Model.SetCooldown(View.FireCooldown);
        }
        
        public void LogAmmoDebug()
        {
            var realCount = Model.AmmoQueue.Count;

            var currentAmmo = Model.PeekNextAmmo(0); 
            var nextAmmo = Model.PeekNextAmmo(1);

            var currentTag = (0 >= realCount) ? "[INF]" : "[REAL]";
            var nextTag    = (1 >= realCount) ? "[INF]" : "[REAL]";

            var currentInfo = $"{currentTag} {currentAmmo.Color}";
            var nextInfo    = $"{nextTag} {nextAmmo.Color}";

            var countInfo = realCount == 0 ? "БЕСКОНЕЧНЫЕ" : realCount.ToString();
            var readyStatus = Model.IsReady ? "ГОТОВ" : $"КД {Model.CurrentCooldown:F1}s";

            Debug.LogWarning($"[Cannon] {readyStatus} | Запас: {countInfo} || Очередь: {currentInfo} -> {nextInfo}");
        }
    }
}