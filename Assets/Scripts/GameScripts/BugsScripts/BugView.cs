using UnityEngine;

namespace GameScripts.BugsScripts
{
    public class BugView : MonoBehaviour
    {
        public GameObject BugPrefab;
        public Transform BugTransform;
        public Animator BugAnimator;
        public string DestroyTrigger;
        public ParticleSystem HitParticles;
        public ParticleSystem DestroyParticles;

        private int _destroyTrigger;

        private void Awake()
        {
            _destroyTrigger = Animator.StringToHash(DestroyTrigger);
        }

        public void DestroyView()
        {
            BugAnimator.SetTrigger(_destroyTrigger);
        }
    }
}