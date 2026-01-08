using GameScripts.CannonScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerSystem : IGameSystem
    {
        public InputPlayerModel Model;
        
        public InputPlayerSystem(GameSystemsHandler context, InputPlayerModel model, Camera camera)
        {
            Model = model;
            
            Model.InitializeModel(context, camera);
        }

        public void InitSystem(GameSystemsHandler context) { }

        public void UpdateSystem(float deltaTime, GameSystemsHandler context)
        {
            Model.UpdateModel(deltaTime, context);
        }
    }
}