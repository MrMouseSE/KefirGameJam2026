using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.BugsScripts;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameScripts.BuildingScripts
{
    public class BuildingModel
    {
        public BuildingSystem System;
        public BuildingView View;
        public List<FloorData> CurrentFloors;
        public bool IsRuined => CurrentFloors.Count == 0;

        private GameSystemsHandler _context;
        private BugSpawnSystem _bugSpawnSystem;

        private int _visualDestructionQueue = 0;
        private bool _isHit;
        private RaycastHit _hitData;
        private string _pendingBugAddress;
        private BuildingColors _pendingBugColor;

        private bool _needRemoveTopFloor;
        
        private float _destructionTimer = 0f;
        private float _destructionInterval = 0.15f;

        public void SetBuildingHitted(RaycastHit hit, string bugAddress, BuildingColors bugColor)
        {
            _isHit = true;
            _hitData = hit;
            _pendingBugAddress = bugAddress;
            _pendingBugColor = bugColor;
        }

        public void InitializeBuilding(BuildingSystem system, BuildingView view, BuildingData data)
        {
            System = system;
            View = view;
            
            CurrentFloors = data.FloorsData;
        }

        public void SetContext(GameSystemsHandler context)
        {
            _context = context;
            _bugSpawnSystem = _context.GetGameSystemByType(typeof(BugSpawnSystem)) as BugSpawnSystem;

        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (_isHit)
            {
                ProcessHit();
                _isHit = false;
            }
            
            if (_visualDestructionQueue > 0)
            {
                _destructionTimer += deltaTime;

                if (_destructionTimer >= _destructionInterval)
                {
                    _destructionTimer = 0f;
                    ExecuteVisualDestruction();
                    _visualDestructionQueue--;
                }
            }
            else if (IsRuined && View.Floors.Count == 0) 
            {
                ConvertToRuins();
            }
        }

        private void ProcessHit()
        {
            
            _bugSpawnSystem.Model.CreateBug(_pendingBugAddress, _hitData.point, this, _pendingBugColor);
            
            if (View.HitParticles != null) 
            {
                View.HitParticles.transform.position = _hitData.point;
                View.HitParticles.Play();
            }
        }

        public void RemoveFloorData()
        {
            if (CurrentFloors.Count > 0)
            {
                CurrentFloors.RemoveAt(CurrentFloors.Count - 1);
            }
        }
        
        public void EnqueueVisualDestruction(int count)
        {
            _visualDestructionQueue += count;
            _destructionTimer = _destructionInterval; 
        }

        private void ExecuteVisualDestruction()
        {
            if (View.Floors.Count > 0)
            {
                var lastIndex = View.Floors.Count - 1;
                var floorView = View.Floors[lastIndex];

                if (floorView.DestroyParticles != null)
                {
                    floorView.DestroyParticles.transform.SetParent(null);
                    floorView.DestroyParticles.Play();
                    Object.Destroy(floorView.DestroyParticles.gameObject, 2f);
                }
            
                View.Floors.RemoveAt(lastIndex);
                Object.Destroy(floorView.gameObject);
            }
            else if (View.DestroyParticles != null)
            {
                View.DestroyParticles.Play();
            }
        }
        
        private void ConvertToRuins()
        {
            // View.SetRuinsSprite(); 

            _context.AddSystemToDelete(System);
            Object.Destroy(View.gameObject);
        }
        
        public BuildingColors GetTopFloorColor()
        {
            if (IsRuined) return default;
            return CurrentFloors.Last().FloorColor;
        }
    }
}