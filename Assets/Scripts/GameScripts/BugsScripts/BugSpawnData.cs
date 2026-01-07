using System;
using UnityEngine;

namespace GameScripts.BugsScripts
{
    [Serializable]
    public class BugSpawnData
    {
        public string Name;
        public GameObject BugPrefab;
        [Tooltip("Чем выше число, тем чаще выпадает этот жук")]
        public float SpawnWeight;
    }
}