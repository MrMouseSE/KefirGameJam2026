using System;
using GameScripts.BuildingScripts;

namespace GameScripts.BugsScripts
{
    public class BugModel
    {
        public BuildingColors BuildingColor;
        
        public BugSystem System;
        public BugView View;

        public BuildingModel TargetBuilding;

        public Action OnBugDestroyed;

        private float _bugLiveTime;

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            _bugLiveTime -= deltaTime;
            if (_bugLiveTime > 0f) return;
        }

        private void OnDestroy()
        {
            View.DestroyView();
        }
    }
}