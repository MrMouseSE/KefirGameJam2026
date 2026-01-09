using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScripts.BuildingScripts
{
    public class FloorView : MonoBehaviour
    {
        public GameObject FloorPrefab;
        public BoxCollider FloorCollider;
        public Transform FloorTransform;
        public SpriteRenderer FloorRenderer;
        public Animation FloorAnimation;
        public AnimationClip FloorDestroyClip;
        public AnimationClip FloorHitClip;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        public FloorAnimationEvents FloorAnimationEvents;
        [HideInInspector] public Sprite DestructionSprite;
        
        public float EatingSpeed = 2.0f;
        
        [HideInInspector] public float FloorHeight = 1.0f;
        [HideInInspector] public float SpawnOffsetX = 0f;
        [HideInInspector] public float BugSpawnOffsetY = 0f;
        
        public void PlayEatAnimation()
        {
            FloorAnimation.Play(FloorHitClip.name);
        }

        public void PlayDestroyAnimation()
        {
            FloorAnimation.Play(FloorDestroyClip.name);
        }
    }
}