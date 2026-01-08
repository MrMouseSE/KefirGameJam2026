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

        public bool IsDead;

        public void Initialize(BugSystem system, BugView view, BuildingModel target)
        {
            System = system;
            View = view;
            TargetBuilding = target;
            IsDead = false;
        }

    }
}