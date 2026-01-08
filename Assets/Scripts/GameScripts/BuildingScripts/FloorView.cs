using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class FloorView : MonoBehaviour
    {
        public GameObject FloorPrefab;
        public Transform FloorTransform;
        public SpriteRenderer FloorRenderer;
        public Animation FloorAnimation;
        public AnimationClip FloodDestroyClip;
        public AnimationClip FloorHitClip;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
    }
}
