using System.Collections.Generic;
using System.Linq;
using GameScripts.BugsScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameScripts.BuildingScripts
{
    public class BuildingModel
    {
        public BuildingSystem System;
        public BuildingView View;
        public List<FloorData> CurrentFloors;
        public bool IsRuined => _currentTopFloorIndex < 0;
        public bool IsBusy => _busyTimer > 0;
        
        private GameSystemsHandler _context;
        private BugSpawnSystem _bugSpawnSystem;
        private int _currentTopFloorIndex;
        private float _busyTimer = 0f;

        private bool _isHit;
        private bool _processingHit = false;
        private string _pendingBugAddress;
        private RaycastHit _hitData;
        private BuildingColors _pendingBugColor;

        public void InitializeBuilding(BuildingSystem system, BuildingView view, BuildingData data)
        {
            System = system;
            View = view;

            CurrentFloors = data.FloorsData;
            _currentTopFloorIndex = CurrentFloors.Count - 1;
        }

        public void SetContext(GameSystemsHandler context)
        {
            _context = context;
            _bugSpawnSystem = _context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;

        }

        public void SetBuildingHitted(RaycastHit hit, string bugAddress, BuildingColors bugColor)
        {
            if (IsRuined || _processingHit || IsBusy) return;
            
            _isHit = true;
            _hitData = hit;
            _pendingBugAddress = bugAddress;
            _pendingBugColor = bugColor;
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (_busyTimer > 0)
            {
                _busyTimer -= deltaTime;
            }
            
            if (_isHit)
            {
                _processingHit = true;
                ProcessHit();
                _isHit = false;
                _processingHit = false;
            }

            if (IsRuined && !IsBusy)
            {
                ConvertToRuins();
            }
        }
        
        private void ProcessHit()
        {
            if (IsRuined) return;

            var topFloorView = View.Floors[_currentTopFloorIndex];
            var topFloorData = CurrentFloors[_currentTopFloorIndex];

            if (topFloorData.FloorColor != _pendingBugColor)
            {
                _bugSpawnSystem.Model.CreateBug(_pendingBugAddress, _hitData.point, this, _pendingBugColor, 0, 0, null);
                return;
            }

            var travelDistance = 0f;
            var floorsEatenCount = 0;

            var floorsToEat = new List<FloorView>();
            
            for (int i = _currentTopFloorIndex; i >= 0; i--)
            {
                if (CurrentFloors[i].FloorColor == _pendingBugColor)
                {
                    travelDistance += View.Floors[i].FloorHeight;
                    View.Floors[i].FloorCollider.enabled = false;
                    floorsToEat.Add(View.Floors[i]);
                    floorsEatenCount++;
                }
                else
                {
                    break;
                }
            }

            if (topFloorView.EatingSpeed > 0)
            {
                _busyTimer = (travelDistance / topFloorView.EatingSpeed) + 0.5f;
            }
            
            var spawnPos = topFloorView.transform.position;
            spawnPos.y += topFloorView.FloorHeight; 
            spawnPos.x += topFloorView.SpawnOffsetX;

            _currentTopFloorIndex -= floorsEatenCount;
            _context.CurrentDestroyedBuildings.DestroyedBuildingsValues.Add(1);

            _bugSpawnSystem.Model.CreateBug(_pendingBugAddress, spawnPos, this, _pendingBugColor, travelDistance, topFloorView.EatingSpeed, floorsToEat);
        }

        private void ConvertToRuins()
        {
            _context.AddSystemToDelete(System);
            _context.RemoveBuilding();
            _context.CurrentDestroyedBuildings.DestroyedBuildingsValues.Add(3);
        }

        public (BuildingColors color, float height, float speed) GetTopFloorInfo()
        {
            var targetIndex = _currentTopFloorIndex + 1;

            if (targetIndex >= CurrentFloors.Count)
            {
                targetIndex = CurrentFloors.Count - 1;
            }
    
            if (targetIndex < 0) targetIndex = 0;

            var data = CurrentFloors[targetIndex];
            var view = View.Floors[targetIndex];

            return (data.FloorColor, view.FloorHeight, view.EatingSpeed);
        }
    }
}