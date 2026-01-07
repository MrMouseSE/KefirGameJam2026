using UnityEngine.InputSystem;

namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerModel
    {
        public PlayerActionsMap PlayerActions;

        public void InitializeModel()
        {
            PlayerActions = new PlayerActionsMap();
            EnableInput();
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
            DisableInput();
            PlayerActions.Dispose();
        }
    }
}