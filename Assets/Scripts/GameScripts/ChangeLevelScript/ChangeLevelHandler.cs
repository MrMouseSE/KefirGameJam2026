using UnityEngine;

namespace GameScripts.ChangeLevelScript
{
    public class ChangeLevelHandler : MonoBehaviour
    {
        private static readonly int Level = Animator.StringToHash("ChangeLevel");
        public Animator ChangeLevelAnimator;
        
        public GameSystemsHandler GameSystemsHandler;

        public void ChangeLevel(GameSystemsHandler gameSystemsHandler)
        {
            GameSystemsHandler = gameSystemsHandler;
            // ChangeLevelAnimator.SetTrigger(Level);
            ChangeLevelAnimationComplete();
        }
        
        //invoke from animation clip
        public void ChangeLevelAnimationComplete()
        {
            GameSystemsHandler.InitGameSystems();
        }
    }
}