using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Descriptions
{
    [CreateAssetMenu(menuName = "Create LevelsDescriptionsHolder", fileName = "LevelsDescriptionsHolder", order = 0)]
    public class LevelsDescriptionsHolder : ScriptableObject
    {
        public List<LevelDescription> LevelDescription;
    }
}