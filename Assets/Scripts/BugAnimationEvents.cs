using System;
using GameScripts.BuildingScripts;
using UnityEngine;

public class BugAnimationEvents : MonoBehaviour
{
    public TestEat TestEat; 
    public BuildingColors BugColor; 
    public ParticleSystem AppearanceEffectRed;
    public ParticleSystem EatEffect;
    public ParticleSystem DeathEffect;

    public void OnDeathEffect()
    {
        DeathEffect.Play();
        EatEffect.Stop();
    }
    
    public void OnEatEffect()
    {
        TestEat.MoveBug();
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
