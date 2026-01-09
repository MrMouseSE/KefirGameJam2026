using System;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class FloorView : MonoBehaviour
    {
        public GameObject FloorPrefab;
        public BoxCollider FloorCollider;
        public Transform FloorTransform;
        public SpriteRenderer FloorRenderer;
        public Animation FloorAnimation;
        public AnimationClip FloodDestroyClip;
        public AnimationClip FloorHitClip;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        public FloorAnimationEvents FloorAnimationEvents;

        public float EatingSpeed = 2.0f;
        
        [HideInInspector] public float FloorHeight = 1.0f;
        [HideInInspector] public float SpawnOffsetX = 0f;
        [HideInInspector] public float BugSpawnOffsetY = 0f;
        
        public void PlayEatAnimation()
        {
            FloorAnimation.clip = FloorHitClip;
            FloorAnimation.Play();
        }

        public void PlayDestroyAnimation()
        {
            FloorAnimation.clip = FloodDestroyClip;
            FloorAnimation.Play();
        }
    }
}