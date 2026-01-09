using System;
using System.Collections.Generic;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.CannonScripts
{
    [Serializable]
    public class BugSpriteProfile
    {
        public BuildingColors Color;
        public Sprite BugSprite;
    }
    
    public class CannonView : MonoBehaviour
    {
        public Transform CannonRoot;
        public Transform FirePoint;
        public ParticleSystem ShootParticles;
        public float FireCooldown = 0.5f;
        public float CannonRotatonSpeed = 150f;
        public SpriteRenderer CurrentBugSprite;
        public SpriteRenderer NextBugSprite;
        public Animator CannonAnimator;
        
        public List<BugSpriteProfile> BugSpritesList;
        
        public string ShootTrigger = "ShootTrigger";
        public event Action OnSwapAnimationEvent;
        
        public void SwapBugReferences()
        {
            (CurrentBugSprite, NextBugSprite) = (NextBugSprite, CurrentBugSprite);
        }
        
        public void TriggerSwapEvent()
        {
            OnSwapAnimationEvent?.Invoke();
        }
        
        public void TriggerCannonShoot()
        {
            CannonAnimator.SetTrigger(ShootTrigger);
        }
    }
}