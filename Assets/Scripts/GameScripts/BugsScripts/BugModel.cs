using System.Collections;
using System.Collections.Generic;
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

        private List<FloorView> _floorsToEat;
        private int _currentFloorIndex;
        private float _nextHideThreshold;
        private FloorView _floorToDamage;

        public void Initialize(BugSystem system, BugView view, BuildingModel target, BuildingColors color,
            GameSystemsHandler context, float travelDistance, float speed, List<FloorView> floorsToEat, float spawnHeightOffset, FloorView floorToDamage)
        {
            System = system;
            View = view;
            TargetBuilding = target;
            BugColor = color;
            _context = context;
            _floorsToEat = floorsToEat;
            _floorToDamage = floorToDamage;
            
            View.BugAnimationEvents.OnStartMovingEvent += HandleStartMoving;
            View.BugAnimationEvents.OnReadyToDestroyEvent += HandleDestroySelf;

            _moveSpeed = speed;
            _currentDistance = 0f;

            if (travelDistance > 0)
            {
                _distanceToTravel = travelDistance + spawnHeightOffset;

                View.transform.position += Vector3.up * spawnHeightOffset;
                View.TriggerAppearAnimation();

                _currentFloorIndex = 0;
                if (_floorsToEat.Count > 0)
                {
                    _nextHideThreshold = spawnHeightOffset + _floorsToEat[0].FloorHeight;
                    
                    SetFloorMasking(_floorsToEat[0]);
                }
            }
            else
            {
                if (_floorToDamage != null && _floorToDamage.DestructionSprite != null)
                {
                    _floorToDamage.FloorRenderer.sprite = _floorToDamage.DestructionSprite;
                }
                
                View.TriggerInstantDeathAnimation();
                _distanceToTravel = 0;
            }
        }

        public void UpdateModel(float deltaTime)
        {
            if (_isDead || !_isMoving) return;

            var step = _moveSpeed * deltaTime;

            if (_floorsToEat != null && _currentFloorIndex < _floorsToEat.Count)
            {
                if (_currentDistance + step >= _nextHideThreshold)
                {
                    var floorToHide = _floorsToEat[_currentFloorIndex];
                    if (floorToHide != null)
                    {
                        floorToHide.PlayDestroyAnimation();
                    }

                    _currentFloorIndex++;

                    if (_currentFloorIndex < _floorsToEat.Count)
                    {
                        _nextHideThreshold += _floorsToEat[_currentFloorIndex].FloorHeight;
                        
                        SetFloorMasking(_floorsToEat[_currentFloorIndex]);
                    }
                }
            }
            
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

        private void SetFloorMasking(FloorView floor)
        {
            floor.FloorRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            
            floor.FloorAnimationEvents.OnAnimationEventTriggered += HandleFloorHideEvent;
            
            floor.PlayEatAnimation();
        }

        private void HandleFloorHideEvent(FloorView floor)
        {
            floor.FloorAnimationEvents.OnAnimationEventTriggered -= HandleFloorHideEvent;
            
            floor.gameObject.SetActive(false);
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

            if (_floorToDamage != null && _floorToDamage.DestructionSprite != null)
            {
                _floorToDamage.FloorRenderer.sprite = _floorToDamage.DestructionSprite;
            }
            
            View.TriggerDeathAnimation();
        }

        private void HandleDestroySelf()
        {
            View.BugAnimationEvents.OnStartMovingEvent -= HandleStartMoving;
            View.BugAnimationEvents.OnReadyToDestroyEvent -= HandleDestroySelf;

            var spawner = _context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;
            spawner.Model.NotifyBugDied(this);

            Debug.Log($"[MODEL] HandleDestroySelf получен! Жук должен был умереть, но мы ему запретили.");
            
            _context.AddSystemToDelete(System);
            Addressables.ReleaseInstance(View.gameObject);
        }
    }
}