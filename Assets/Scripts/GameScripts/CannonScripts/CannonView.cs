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
        public Texture2D Gradient;
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
        public SpriteRenderer LichinkasBlyob;
        
        public List<BugSpriteProfile> BugSpritesList;
        
        public string ShootTrigger = "ShootTrigger";
        public string ReloadTrigger = "ReloadTrigger";
        public event Action OnSwapAnimationEvent;
        
        public void SwapBugReferences()
        {
            (CurrentBugSprite.name, NextBugSprite.name) = (NextBugSprite.name, CurrentBugSprite.name);
        }
        
        public void TriggerSwapEvent()
        {
            OnSwapAnimationEvent?.Invoke();
        }
        
        public void TriggerCannonShoot()
        {
            CannonAnimator.SetTrigger(ShootTrigger);
        }
        
        public void TriggerCannonReload()
        {
            CannonAnimator.SetTrigger(ReloadTrigger);
        }
    }
}