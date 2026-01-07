using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Descriptions
{
    [CreateAssetMenu(menuName = "Create LevelDescriptions", fileName = "LevelDescriptions", order = 0)]
    public class LevelDescription : ScriptableObject
    {
        public int Level;
        public Vector2Int BuildingsCount;
        public List<BuildingsDescriptions> BuildingsDescriptions;

        public void OnEnable()
        {
            BuildingsDescriptions = BuildingsDescriptions.OrderByDescending(x => x.BuildingChancePercent).ToList();
        }

        public int GetBuildingsCount()
        {
            return Random.Range(BuildingsCount.x, BuildingsCount.y);
        }

        public BuildingsDescriptions GetBuildingDescriptionByChance(float chance)
        {
            for (int i = 0; i < BuildingsDescriptions.Count; i++)
            {
                if (BuildingsDescriptions[i].BuildingChancePercent >= chance)
                {
                    return BuildingsDescriptions[i];
                }
            }
            return BuildingsDescriptions[^1];
        }
    }
}