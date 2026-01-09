using System.Collections;
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

        private GameSystemsHandler _context;
        
        private bool _isMoving = false; 
        private bool _isDead = false;

        private float _moveSpeed;
        private float _distanceToTravel;
        private float _currentDistance;

        public void Initialize(BugSystem system, BugView view, BuildingModel target, BuildingColors color, GameSystemsHandler context, float travelDistance, float speed)
        {
            System = system;
            View = view;
            TargetBuilding = target;
            BugColor = color;
            _context = context;

            View.BugAnimationEvents.OnStartMovingEvent += HandleStartMoving;
            View.BugAnimationEvents.OnDestroySelfEvent += HandleDestroySelf;
            
            _moveSpeed = speed;
            _currentDistance = 0f;

            if (travelDistance > 0)
            {
                _distanceToTravel = travelDistance + View.SpawnBugHeightOffset;
                View.transform.position += Vector3.up * View.SpawnBugHeightOffset;
                View.TriggerAppearAnimation();
            }
            else
            {
                View.TriggerInstantDeathAnimation();
                _distanceToTravel = 0;
            }
        }

        public void UpdateModel(float deltaTime)
        {
            if (_isDead || !_isMoving) return;

            var step = _moveSpeed * deltaTime;

            if (_currentDistance + step >= _distanceToTravel)
            {
                var remaining = _distanceToTravel - _currentDistance;
                View.transform.Translate(Vector3.down * remaining);
                _currentDistance += remaining;
                
                StartDeathSequence();
            }
            else
            {
                View.transform.Translate(Vector3.down * step);
                _currentDistance += step;
            }
        }

        private void HandleStartMoving()
        {
            if (_isDead) return;
            _isMoving = true;
        }

        private void StartDeathSequence()
        {
            _isMoving = false;
            _isDead = true;
            
            View.TriggerDeathAnimation();
        }

        private void HandleDestroySelf()
        {
            View.BugAnimationEvents.OnStartMovingEvent -= HandleStartMoving;
            View.BugAnimationEvents.OnDestroySelfEvent -= HandleDestroySelf;
            
            var spawner = _context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;
            spawner.Model.NotifyBugDied(this);

            _context.AddSystemToDelete(System);
            Addressables.ReleaseInstance(View.gameObject);
        }
    }
}