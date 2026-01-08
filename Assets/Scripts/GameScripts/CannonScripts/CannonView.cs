using UnityEngine;

namespace GameScripts.CannonScripts
{
    public class CannonView : MonoBehaviour
    {
        public Transform CannonRoot;
        public Transform FirePoint;
        public ParticleSystem ShootParticles;
        public float FireCooldown = 0.5f;
        public float CannonRotatonSpeed = 150f;

    }
}