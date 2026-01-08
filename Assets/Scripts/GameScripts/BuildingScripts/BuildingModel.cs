using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingModel
    {
        public BuildingSystem System;
        public BuildingView View;
        
        public BuildingData Data;

        private int _buildingHitPoints;

        public void InitializeBuilding(BuildingSystem system, BuildingView view)
        {
            System = system;
            View = view;
            _buildingHitPoints = View.Floors.Count-1;
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            if (!context.IsBugSpawned) return;
            if (context.CurrentBug.TargetBuilding.View != View) return;
            if (Data.FloorsData[_buildingHitPoints].FloorColor != context.CurrentBug.BugColor)
            {
                context.CurrentBug.DestroyBug();
                View.Floors[_buildingHitPoints].FloorAnimation.Play(View.Floors[_buildingHitPoints].FloorHitClip.name);
                View.Floors[_buildingHitPoints].HitParticles.Play();
                return;
            }
            context.CurrentBug.BugAnimationPlay();
            View.Floors[_buildingHitPoints].FloorAnimation.Play(View.Floors[_buildingHitPoints].FloodDestroyClip.name);
            View.Floors[_buildingHitPoints].DestroyParticles.Play();
            _buildingHitPoints --;
        }

        public Transform GetCurrentFloorTransform()
        {
            return View.Floors[_buildingHitPoints].FloorTransform;
        }
    }
}