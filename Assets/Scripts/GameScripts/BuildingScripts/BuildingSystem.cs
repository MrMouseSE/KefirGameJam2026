using System.Linq;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingSystem : IGameSystem
    {
        public BuildingModel Model;
        public BuildingView View;
        private GameSystemsHandler _context;

        public BuildingSystem(BuildingModel model, BuildingView view, BuildingData data)
        {
            Model = model;
            View = view;
            Model.InitializeBuilding(this, view, data);
        }
        
        public void InitSystem(GameSystemsHandler context)
        {
            _context = context;
            View.OnHitBuilding += OnHit;
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }

        private void OnHit(RaycastHit hit, GameObject bugPrefab)
        {
            _context.CreateAndRegisterBug(bugPrefab, hit.point, this.Model);
        }
        
        public bool HasFloors => Model.CurrentFloors.Count > 0;
        
        public BuildingColors GetTopFloorColor()
        {
            return !HasFloors ? default(BuildingColors) : Model.CurrentFloors.Last().FloorColor;
        }
        
        public void RemoveTopFloor()
        {
            if (!HasFloors) return;

            var floorToRemove = Model.CurrentFloors.Last();
            Model.CurrentFloors.RemoveAt(Model.CurrentFloors.Count - 1);

            View.RemoveTopFloorView();
        }

        public void DemolishBuilding()
        {
            while(HasFloors)
            {
                RemoveTopFloor();
            }
        }

        private void ConvertToRuins(GameSystemsHandler context)
        {
            // View.SetRuinsSprite(); 
            
            context.AddSystemToDelete(this);
            Model = null;
        }
    }
}