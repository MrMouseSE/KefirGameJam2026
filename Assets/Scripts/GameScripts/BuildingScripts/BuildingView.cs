using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingView : MonoBehaviour
    {
        public GameObject BuildingPrefab;
        public Transform BuildingTransform;
        public Transform FloorsTransform;
        public List<FloorView> Floors;
        
        [Space(10)]
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
    }
}
