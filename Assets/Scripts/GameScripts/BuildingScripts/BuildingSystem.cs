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
            
            if (data != null)
            {
                Model.CurrentFloors = data.FloorsData;
            }
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

        private void OnHit(RaycastHit hit, string bugAddress, BuildingColors bugColor)
        {
            _context.CreateAndRegisterBug(bugAddress, hit.point, this.Model, bugColor);
        }
        
        public bool HasFloors => Model != null && Model.CurrentFloors != null && Model.CurrentFloors.Count > 0;
        
        public BuildingColors GetTopFloorColor()
        {
            return !HasFloors ? default(BuildingColors) : Model.CurrentFloors.Last().FloorColor;
        }
        
        public void RemoveTopFloor()
        {
            if (!HasFloors) return;

            Model.CurrentFloors.RemoveAt(Model.CurrentFloors.Count - 1);

            View.RemoveTopFloorView();

            if (!HasFloors)
            {
                ConvertToRuins(_context);
            }
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