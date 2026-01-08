using System;
using GameScripts.BuildingScripts;

namespace GameScripts.BugsScripts
{
    public class BugModel
    {
        public BuildingColors BugColor;
        
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

        public void BugAnimationPlay()
        {
            //TODO: bug eating animation
        }

        public void DestroyBug()
        {
            OnBugDestroyed?.Invoke();
            OnDestroy();
        }

        private void OnDestroy()
        {
            View.DestroyView();
        }

        private void SetBugOnBuilding()
        {
            View.BugTransform.position = TargetBuilding.GetCurrentFloorTransform().position;
        }
    }
}