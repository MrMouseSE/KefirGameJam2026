using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class FloorHandler : MonoBehaviour
    {
        public GameObject FloorPrefab;
        public Transform FloorTransform;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
    }
}
