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

        [Space(10)]
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;
        
        public void ReceiveHit(RaycastHit hit, string bugAddress, BuildingColors color)
        {
            OnHitBuilding?.Invoke(hit, bugAddress, color);
        }
        
        public void RemoveTopFloorView()
        {
            if (Floors == null || Floors.Count == 0) return;

            var lastIndex = Floors.Count - 1;
            var floorView = Floors[lastIndex];

            if (floorView != null && floorView.DestroyParticles != null)
            {
                floorView.DestroyParticles.transform.SetParent(null);
                floorView.DestroyParticles.Play();
                Destroy(floorView.DestroyParticles.gameObject, 2f);
            }
            else
            {
                PlayDestroyEffects();
            }

            Floors.RemoveAt(lastIndex);

            if (floorView != null)
            {
                Destroy(floorView.gameObject);
            }
        }

        public void PlayDestroyEffects()
        {
            DestroyParticles.Play();

            // Destroy(DestroyParticles.gameObject, DestroyParticles.main.duration);
        }
    }
}
