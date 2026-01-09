using System;
using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScripts.BugsScripts
{
    public class BugView : MonoBehaviour
    {
        [HideInInspector] public BuildingColors BugColor;
        
        public GameObject BugPrefab;
        public Transform BugTransform;
        public BugAnimationEvents BugAnimationEvents;
        public string AppearTriggerName = "AppearanceTrigger";
        public string DieTriggerName = "DeathTrigger";
        public string InstantDeathTriggerName = "InstantDeathTrigger";
        public float SpawnBugHeightOffset = 2.0f;

        public void TriggerAppearAnimation()
        {
            BugAnimationEvents.BugAnimator.SetTrigger(AppearTriggerName);
        } 
            
        public void TriggerInstantDeathAnimation()
        {
            BugAnimationEvents.BugAnimator.SetTrigger(InstantDeathTriggerName);
        } 
        
        public void TriggerDeathAnimation()
        {
            BugAnimationEvents.BugAnimator.SetTrigger(DieTriggerName);
        }
    }
}