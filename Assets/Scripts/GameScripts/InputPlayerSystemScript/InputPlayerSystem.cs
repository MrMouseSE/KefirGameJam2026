using GameScripts.CannonScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerSystem : IGameSystem
    {
        public InputPlayerModel Model;
        private CannonSystem _cannonSystem;
        private Camera _camera;
        
        public InputPlayerSystem(InputPlayerModel model, Camera camera)
        {
            Model = model;
            _camera = camera;
            
            Model.InitializeModel();
        }

        public void InitSystem(GameSystemsHandler context)
        {
            var system = context.GetGameSystemByType(typeof(CannonSystem));
            if (system is CannonSystem cannon)
            {
                _cannonSystem = cannon;
            }
            
            Model.PlayerActions.Player.Attack.performed += OnAttackPerformed;
            Model.PlayerActions.Player.RemoveAmmo.performed += OnRemovePerformed;
        }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
        }

        private void OnRemovePerformed(InputAction.CallbackContext ctx)
        {
            if (_cannonSystem.Model.AmmoQueue.Count == 0) return;
            
            _cannonSystem.Model.AmmoQueue.Dequeue();
            _cannonSystem.LogAmmoDebug();
        }

        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            if (_cannonSystem == null || _camera == null) return;

            var mousePos = Mouse.current.position.ReadValue();
            
            var ray = _camera.ScreenPointToRay(mousePos);
            
            _cannonSystem.TryFire(ray);
        }
        
        public void DisposeInput()
        {
            Model.PlayerActions.Player.Attack.performed -= OnAttackPerformed;
            Model.DisposeInput();
        }
    }
}