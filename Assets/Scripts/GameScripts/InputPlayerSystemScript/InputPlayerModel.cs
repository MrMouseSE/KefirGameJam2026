using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerModel
    {
        public InputPlayerSystem System;
        private PlayerActionsMap _playerActionsMap;
        private Camera _mainCamera;
        
        public void InitializeModel(InputPlayerSystem system, Camera camera)
        {
            System = system;
            _mainCamera = camera;
            
            _playerActionsMap = new PlayerActionsMap();
            EnableActionMap();
        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            
        }
        
        private void EnableActionMap()
        {
            _playerActionsMap.Enable();
            _playerActionsMap.Player.Attack.performed += OnAttack;
            _playerActionsMap.Player.Remove.performed += OnRemoveBug;
        }

        private void DisableActionMap()
        {
            _playerActionsMap.Player.Attack.performed -= OnAttack;
            _playerActionsMap.Disable();
        }

        private void DisposeActionMap()
        {
            DisableActionMap();
            _playerActionsMap.Dispose();
        }
        
        private void OnRemoveBug(InputAction.CallbackContext obj)
        {
            Debug.LogError($"[DEBUG] Gun Reloaded. Current bug is dead.");
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            ShootRayFromCamera();
        }
        
        private void ShootRayFromCamera()
        {
            if (_mainCamera == null) return;
            
            var mouseScreenPos = Mouse.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(mouseScreenPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                var targetBuilding = hit.collider.GetComponentInParent<BuildingView>();

                if (targetBuilding != null)
                {
                    Debug.Log($"[DEBUG] Hit Building: {targetBuilding.name}");
                    
                    Debug.DrawLine(ray.origin, hit.point, UnityEngine.Color.green, 2f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, hit.point, UnityEngine.Color.red, 2f);
                }
            }
            else
            {
                var targetPoint = ray.GetPoint(1000f);
                
                Debug.DrawLine(ray.origin, targetPoint, UnityEngine.Color.white, 2f);
            }
        }
    }
}