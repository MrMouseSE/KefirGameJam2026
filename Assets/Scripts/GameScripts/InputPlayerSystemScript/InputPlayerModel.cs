namespace GameScripts.InputPlayerSystemScript
{
    public class InputPlayerModel
    {
        public InputPlayerSystem System;

        public void InitializeModel(InputPlayerSystem system)
        {
            System = system;
        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            
        }
    }
}