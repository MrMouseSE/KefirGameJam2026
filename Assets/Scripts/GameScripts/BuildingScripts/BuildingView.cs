using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    public class BuildingView : MonoBehaviour
    {
        public Action<RaycastHit, GameObject> OnHitBuilding;
        
        public GameObject BuildingPrefab;
        public Transform BuildingTransform;
        public Transform FloorsTransform;
        public List<FloorView> Floors;

        [Space(10)]
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        
        public void ReceiveHit(RaycastHit hit, GameObject bugPayload)
        {
            OnHitBuilding?.Invoke(hit, bugPayload);
        }
        
        public void RemoveTopFloorView()
        {
            if (Floors.Count == 0) return;

            var lastIndex = Floors.Count - 1;
            var floorView = Floors[lastIndex];

            if (floorView.DestroyParticles != null)
            {
                floorView.DestroyParticles.transform.SetParent(null);
                floorView.DestroyParticles.Play();
                Destroy(floorView.DestroyParticles.gameObject, 1f);
            }

            Floors.RemoveAt(lastIndex);
            Destroy(floorView.gameObject);
        }

        public void PlayDestroyEffects()
        {
            if (DestroyParticles == null) return;
            
            DestroyParticles.transform.SetParent(null); 
            DestroyParticles.Play();
                
            Destroy(DestroyParticles.gameObject, DestroyParticles.main.duration);
        }
    }
}
