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
        public Animator BugAnimator;
        public string DieTriggerName = "Die";
        public float SpawnBugHeightOffset = 2.0f;

        public Action OnStartMovingEvent;
        public Action OnDestroySelfEvent;

        public void StartMoving()
        {
            OnStartMovingEvent?.Invoke();
        }

        public void DestroySelf()
        {
            OnDestroySelfEvent?.Invoke();
        }

        public void TriggerDeathAnimation()
        {
            BugAnimator.SetTrigger(DieTriggerName);
        }
    }
}