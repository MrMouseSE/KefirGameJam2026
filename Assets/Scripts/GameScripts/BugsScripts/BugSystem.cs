using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameScripts.BugsScripts
{
    public class BugSystem : IGameSystem
    {
        public BugModel Model;
        public BugView View;
        private GameSystemsHandler _context;
        
        //DEBUG
        private float _debugTimer = 0f;
        private bool _hasAttacked = false;
        
        public BugSystem(BugModel model, BugView view)
        {
            Model = model;
            View = view;
        }

        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;
            
            View.OnAttackAnimationFinished += OnAnimationEvent;

            View.PlaySpawnAnimation();
        }

        private void OnAnimationEvent()
        {
            if (Model.IsDead) return;

            var targetBuilding = Model.TargetBuilding;

            if (targetBuilding == null || targetBuilding.System == null)
            {
                KillBug();
                return;
            }

            while (targetBuilding.System.HasFloors)
            {
                var topFloorColor = targetBuilding.System.GetTopFloorColor();

                if (topFloorColor == Model.BuildingColor)
                {
                    targetBuilding.System.RemoveTopFloor();
                }
                else
                {
                    KillBug();
                    return;
                }
            }

            KillBug();
        }
        
        private void KillBug()
        {
            if (Model.IsDead) return;
            
            Model.IsDead = true;
            _context.AddSystemToDelete(this);
            
            if (_context.CurrentBug == this.Model) 
            {
                _context.OnBugDied();
            }

            if (View == null) return;
            
            View.OnAttackAnimationFinished = null; 
            Addressables.ReleaseInstance(View.gameObject);
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            if (_hasAttacked) return;
            
            _debugTimer += deltaTime;
            if (!(_debugTimer > 1.0f)) return;
            
            _hasAttacked = true;
            Debug.Log("[DEBUG] Симуляция конца анимации атаки");
            OnAnimationEvent();
        }
    }
}