using System;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.BugsScripts
{
    public class BugView : MonoBehaviour
    {
        public GameObject BugPrefab;
        public Transform BugTransform;
        public Animator BugAnimator;
        public string DestroyTrigger;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        public ParticleSystem DeathSplatterParticles;
        [HideInInspector] public BuildingColors BugColor;
        public bool IsAttackAnimationFinished;

        public void OnAnimationFinishTrigger()
        {
            IsAttackAnimationFinished = true;
        }
    }
}