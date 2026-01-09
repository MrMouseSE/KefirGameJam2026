using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameScripts.BuildingScripts;

public class BugAnimationEvents : MonoBehaviour
{
    public Animator BugAnimator;
    public BuildingColors CurrentBugColor;
    public List<BugEffectsProfile> EffectsProfiles;

    private Dictionary<BuildingColors, BugEffectsProfile> _effectsMap;

    public Action OnStartMovingEvent;
    public Action OnDestroySelfEvent;
    public Action OnReadyToDestroyEvent;

    private void Awake()
    {
        _effectsMap = EffectsProfiles.ToDictionary(bugEffectsProfile => bugEffectsProfile.Color, bugEffectsProfile => bugEffectsProfile);
    }

    public void OnDeathEffect()
    {
        // OnDestroySelfEvent?.Invoke();
        PlayEffect(profile => profile.DeathEffect);
    }

    public void OnReadyToDestroy()
    {
        OnReadyToDestroyEvent?.Invoke();
    }

    public void OnEatEffect()
    {
        OnStartMovingEvent?.Invoke();
        PlayEffect(profile => profile.EatEffect);
    }

    public void OnAppearanceEffect()
    {
        PlayEffect(profile => profile.AppearanceEffect);
    }

    private void PlayEffect(Func<BugEffectsProfile, ParticleSystem> effectSelector)
    {
        if (!_effectsMap.TryGetValue(CurrentBugColor, out var profile)) return;
        
        var effectToPlay = effectSelector(profile);
        effectToPlay.Play();
    }
}

[Serializable]
public class BugEffectsProfile
{
    public BuildingColors Color;
    public ParticleSystem AppearanceEffect;
    public ParticleSystem EatEffect;
    public ParticleSystem DeathEffect;
}