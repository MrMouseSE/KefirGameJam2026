using System;
using System.Linq;
using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

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

        private GameSystemsHandler _context;
        private bool _isDead;
        
        //DEBUG
        private float _timer;
        private float _delay = 1.2f;

        public void Initialize(BugSystem system, BugView view, BuildingModel target, BuildingColors color, GameSystemsHandler context)
        {
            System = system;
            View = view;
            TargetBuilding = target;
            BugColor = color;
            _context = context;

            _timer = 0;
            
            ApplyColor();
        }

        public void UpdateModel(float deltaTime)
        {
            if (_isDead) return;

            _timer += deltaTime;
            
            // if (View.IsAttackAnimationFinished)
            // {
                // EatFloors();
                // View.IsAttackAnimationFinished = false;
            // }
            
            //DEBUG
            if (_timer >= _delay)
            {
                EatFloors();
            }
        }

        private void ApplyColor() { }

        private void EatFloors()
        {
            if (TargetBuilding == null || TargetBuilding.IsRuined)
            {
                Die(false);
                return;
            }
            
            var floorsEatenCount = 0;

            while (!TargetBuilding.IsRuined)
            {
                var topColor = TargetBuilding.GetTopFloorColor();

                if (topColor == BugColor)
                {
                    TargetBuilding.RemoveFloorData(); 
                    floorsEatenCount++;
                }
                else
                {
                    Die(true);
                    break; 
                }
            }

            if (floorsEatenCount > 0)
            {
                TargetBuilding.EnqueueVisualDestruction(floorsEatenCount);
            }
    
            if (!_isDead) Die(false);
        }

        private void Die(bool exploded)
        {
            _isDead = true;
            
            if (exploded && View.DeathSplatterParticles != null)
            {
                View.DeathSplatterParticles.transform.SetParent(null);
                View.DeathSplatterParticles.Play();
                Object.Destroy(View.DeathSplatterParticles.gameObject, 2f);
            }
            
            var spawner = _context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;
            spawner.Model.NotifyBugDied(this);

            _context.AddSystemToDelete(System);
            
            Addressables.ReleaseInstance(View.gameObject);
        }

    }
}