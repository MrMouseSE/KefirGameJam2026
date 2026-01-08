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
        
        public float FloorHeight = 1.0f;
        public float EatingSpeed = 2.0f;
        public float SpawnOffsetX = 0f;
    }
}
