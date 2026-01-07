using GameScripts.BugsScripts;
using UnityEngine;
using GameScripts.BuildingScripts; // Нужно для BuildingView

namespace GameScripts.CannonScripts
{
    public class CannonSystem : IGameSystem
    {
        public CannonModel Model;
        private GameSystemsHandler _context;

        public CannonSystem(CannonModel model)
        {
            Model = model;
        }

        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
        }

        public void TryFire(Ray ray)
        {
            LogAmmoDebug();
            
            if (Model.AmmoQueue.Count == 0) return;

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            
            var buildingView = hit.collider.GetComponentInParent<BuildingView>();

            if (buildingView == null) return;

            var bugPrefab = Model.AmmoQueue.Dequeue();
            
            buildingView.ReceiveHit(hit, bugPrefab);
        }

        private void LogAmmoDebug()
        {
            if (Model.AmmoQueue.Count == 0)
            {
                Debug.LogWarning("[Cannon System] ПУСТО! Нет патронов.");
                return;
            }

            var ammoArray = Model.AmmoQueue.ToArray();

            GameObject currentBugGO = ammoArray[0];
            string currentInfo = GetBugInfo(currentBugGO);

            string nextInfo = "НЕТ (Последний патрон)";
            if (ammoArray.Length > 1)
            {
                GameObject nextBugGO = ammoArray[1];
                nextInfo = GetBugInfo(nextBugGO);
            }

            Debug.LogWarning($"[Cannon System] ЗАРЯЖЕН: {currentInfo} || СЛЕДУЮЩИЙ: {nextInfo} || Всего в очереди: {Model.AmmoQueue.Count}");
        }
        
        private string GetBugInfo(GameObject go)
        {
            if (go == null) return "null";
            
            var view = go.GetComponent<BugView>();
            if (view != null)
            {
                return $"Жук [{view.BugColor}]";
            }
            
            return go.name;
        }
    }
}