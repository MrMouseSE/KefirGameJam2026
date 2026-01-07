using UnityEngine;

namespace GameScripts.CannonScripts
{
    public class CannonView : MonoBehaviour
    {
        public Transform CannonRoot;
        public Transform FirePoint;
        public ParticleSystem ShootParticles;
        public float FireCooldown = 0.5f;
        
        public void PlayFireEffects()
        {
            if (ShootParticles != null)
            {
                ShootParticles.Play();
            }
        }

        public void RotateCannon(Vector3 lookTarget)
        {
            if (CannonRoot == null) return;
            
            CannonRoot.LookAt(lookTarget);
        }
    }
}