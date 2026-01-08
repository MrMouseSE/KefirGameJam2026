using System.Collections.Generic;
using System.Linq;
using GameScripts.BuildingScripts;
using GameScripts.Descriptions;
using UnityEngine;

namespace GameScripts.CannonScripts
{
    public class CannonModel
    {
        public Queue<AmmoData> AmmoQueue = new();

        private List<BuildingColors> _cachedWeightedColors = new List<BuildingColors>();
        private string _cachedDefaultAddress;

        public float CurrentCooldown { get; private set; }
        public bool IsReady => CurrentCooldown <= 0;

        public void LoadLevelAmmo(LevelDescription levelDescription, string defaultBugAddress)
        {
            AmmoQueue.Clear();
            _cachedWeightedColors.Clear();
            _cachedDefaultAddress = defaultBugAddress;
            CurrentCooldown = 0;

            var uniqueColors = levelDescription.BuildingsDescriptions
                .SelectMany(buildingDesc => buildingDesc.BuildingSpriteColorPairs
                    .SelectMany(pair => pair.BuildingColors))
                .Distinct();

            _cachedWeightedColors.AddRange(uniqueColors);
            
            if (_cachedWeightedColors.Count == 0) _cachedWeightedColors.Add(BuildingColors.Red);

            var totalBugsToSpawn = levelDescription.GetBugsCount();

            for (var i = 0; i < totalBugsToSpawn; i++)
            {
                AddRandomAmmoToQueue();
            }
        }

        private void AddRandomAmmoToQueue()
        {
            var randomColor = _cachedWeightedColors[Random.Range(0, _cachedWeightedColors.Count)];

            var ammo = new AmmoData
            {
                BugAddress = _cachedDefaultAddress,
                Color = randomColor
            };

            AmmoQueue.Enqueue(ammo);
        }

        public AmmoData GetNextAmmo()
        {
            if (AmmoQueue.Count == 0)
            {
                AddRandomAmmoToQueue();
            }

            return AmmoQueue.Dequeue();
        }

        public AmmoData PeekNextAmmo(int offset = 0)
        {
            while (AmmoQueue.Count <= offset)
            {
                AddRandomAmmoToQueue();
            }

            return AmmoQueue.ElementAt(offset);
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= deltaTime;
            }
        }

        public void SetCooldown(float duration)
        {
            CurrentCooldown = duration;
        }
    }
}