using System;
using GameScripts.BuildingScripts;
using UnityEngine;

public class BugAnimationEvents : MonoBehaviour
{
    public Animator BugAnimator;
    public TestEat TestEat; 
    public BuildingColors BugColor; 
    public ParticleSystem AppearanceEffectRed;
    public ParticleSystem EatEffect;
    public ParticleSystem DeathEffect;

    public Action OnStartMovingEvent;
    public Action OnDestroySelfEvent;
    
    public void OnDeathEffect()
    {
        OnDestroySelfEvent?.Invoke();
        EatEffect.Stop();
    }
    
    public void OnEatEffect()
    {
        OnStartMovingEvent?.Invoke();
        EatEffect.Play();
    }

    public void OnAppearanceEffect()
    {
        switch (BugColor)
        {
            case BuildingColors.Red:
                AppearanceEffectRed.Play();
                break;
            case BuildingColors.Blue:
                break;
            case BuildingColors.Green:
                break;
            case BuildingColors.Yellow:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        } 
    }
}
