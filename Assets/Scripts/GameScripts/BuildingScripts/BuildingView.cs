using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingView : MonoBehaviour
    {
        public Action<RaycastHit, string, BuildingColors> OnHitBuilding;
        
        public GameObject BuildingPrefab;
        public Transform BuildingTransform;
        public Transform FloorsTransform;
        public List<FloorView> Floors;
        public BuildingModel Model;

        [Space(10)]
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
    }
}
