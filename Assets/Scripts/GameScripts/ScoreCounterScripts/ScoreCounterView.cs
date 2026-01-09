using TMPro;
using UnityEngine;

namespace GameScripts.ScoreCounterScripts
{
    public class ScoreCounterView : MonoBehaviour
    {
        public GameObject ScorePrefab;
        public Transform ScoreTransform;
        public TMP_Text ScoreText;
        public TMP_Text HealthText;
        public int MaxHealth;
    }
}