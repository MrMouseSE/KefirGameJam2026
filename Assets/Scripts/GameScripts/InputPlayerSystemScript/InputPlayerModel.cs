using UnityEngine;
using UnityEngine.InputSystem;

namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerModel
    {
        public PlayerActionsMap PlayerActions;
        
        private Camera _camera;
        private GameSystemsHandler _context;
        private bool _onAttackButtonPressed;
        private bool _onReloadButtonPressed;
        private Vector2 _currentMousePosition;
        
        public bool GetAttackButtonValue() => _onAttackButtonPressed;
        public bool GetReloadButtonValue() => _onReloadButtonPressed;
        public Vector2 GetMousePosition() => _currentMousePosition;
        public Camera GetMainCamera() => _camera;
        public void ResetAttackButtonValue() => _onAttackButtonPressed = false;
        public void ResetReloadButtonValue() => _onReloadButtonPressed = false;

        public void InitializeModel(GameSystemsHandler context, Camera camera)
        {
            _camera = camera;
            _context = context;
            
            PlayerActions = new PlayerActionsMap();
            EnableInput();
                        
            PlayerActions.Player.Attack.performed += OnAttackPerformed;
            PlayerActions.Player.RemoveAmmo.performed += OnReloadPerformed;
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            _currentMousePosition = Mouse.current.position.ReadValue();
        }
        
        public void EnableInput()
        {
            PlayerActions.Enable();
        }

        public void DisableInput()
        {
            PlayerActions.Disable();
        }

        public void DisposeInput()
        {
            PlayerActions.Player.Attack.performed -= OnAttackPerformed;
            PlayerActions.Player.RemoveAmmo.performed -= OnReloadPerformed;
            PlayerActions.Dispose();
        }

        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            _onAttackButtonPressed = true;
        }

        private void OnReloadPerformed(InputAction.CallbackContext ctx)
        {
            _onReloadButtonPressed = true;
        }
    }
}