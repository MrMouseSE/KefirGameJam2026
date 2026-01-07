using System;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.BugsScripts
{
    public class BugView : MonoBehaviour
    {
        public Action OnAttackAnimationFinished;
        
        public GameObject BugPrefab;
        public Transform BugTransform;
        public Animator BugAnimator;
        public string DestroyTrigger;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        [HideInInspector] public BuildingColors BugColor;
        private int _destroyTrigger;

        private void Awake()
        {
            _destroyTrigger = Animator.StringToHash(DestroyTrigger);
        }

        public void DestroyView()
        {
            BugAnimator.SetTrigger(_destroyTrigger);
        }
        
        public void SetColor(BuildingColors colorType)
        {
            BugColor = colorType;
        }
        
        public void OnAnimationFinishTrigger()
        {
            OnAttackAnimationFinished?.Invoke();
        }
        
        public void PlaySpawnAnimation()
        {
            
        }
    }
}