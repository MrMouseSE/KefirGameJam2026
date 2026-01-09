using System.Collections.Generic;
using System.Linq;
using GameScripts.BuildingScripts;
using GameScripts.Descriptions;
using GameScripts.InputPlayerSystemScript;
using UnityEngine;

namespace GameScripts.CannonScripts
{
    public class CannonModel
    {
        private const string DEFAULT_BUG_KEY = "DefaultBug";

        public Queue<AmmoData> AmmoQueue = new();
        public CannonView View;
        public bool IsReady => CurrentCooldown <= 0;

        private float CurrentCooldown { get; set; }
        private List<BuildingColors> _cachedWeightedColors = new List<BuildingColors>();
        private Dictionary<BuildingColors, SpriteColorByBuildingColor> _visualsMap;
        private string _cachedDefaultAddress;
        private InputPlayerModel _inputPlayerModel;

        public void Initialize(GameSystemsHandler context, CannonView view)
        {
            View = view;
            var inputSystem = (InputPlayerSystem)context.GetGameSystemByType(typeof(InputPlayerSystem));

            _inputPlayerModel = inputSystem.Model;

            var levelData = context.CurrentLevelDescription;
            LoadLevelAmmo(levelData, DEFAULT_BUG_KEY);
            InitializeVisualsMap(levelData);
            UpdateAmmoVisuals();
        }

        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            UpdateCooldown(deltaTime);

            var camera = _inputPlayerModel.GetMainCamera();
            var mousePos = _inputPlayerModel.GetMousePosition();
            var currentRay = camera.ScreenPointToRay(mousePos);

            Aim(currentRay, deltaTime);

            if (_inputPlayerModel.GetAttackButtonValue())
            {
                TryFire(currentRay);
            }

            if (_inputPlayerModel.GetReloadButtonValue())
            {
                RemoveCurrentAmmo();
            }
        }

        private void PlayFireEffects()
        {
            // <summary>
            // Раскомментить когда будет эффект
            // </summary>

            // View.ShootParticles.Play();
        }

        private void Aim(Ray ray, float deltaTime)
        {
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100f);
            }

            RotateCannon(targetPoint, deltaTime);
        }

        private void RotateCannon(Vector3 lookTarget, float deltaTime)
        {
            var direction = lookTarget - View.CannonRoot.position;

            if (direction == Vector3.zero) return;

            var targetRotation = Quaternion.LookRotation(direction);

            View.CannonRoot.rotation = Quaternion.RotateTowards(
                View.CannonRoot.rotation,
                targetRotation,
                View.CannonRotatonSpeed * deltaTime
            );
        }

        private void TryFire(Ray ray)
        {
            if (!IsReady)
            {
                Debug.LogError("[DEBUG] Перезарядка");
                _inputPlayerModel.ResetAttackButtonValue();
                return;
            }

            Vector3 targetPoint;

            targetPoint = Physics.Raycast(ray, out RaycastHit aimHit) ? aimHit.point : ray.GetPoint(100f);

            var directionToTarget = (targetPoint - View.CannonRoot.position).normalized;

            var angle = Vector3.Angle(View.CannonRoot.forward, directionToTarget);

            if (angle > 5.0f)
            {
                Debug.LogWarning("Пушка еще наводится!");
                return;
            }

            _inputPlayerModel.ResetAttackButtonValue();
            LogAmmoDebug();

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var buildingView = hit.collider.GetComponentInParent<BuildingView>();
            if (buildingView == null) return;

            if (buildingView.Model.IsBusy)
            {
                Debug.LogWarning("[Cannon] Дом занят другим жуком. Выстрел отменен.");
                return;
            }

            // View.PlayFireEffects();

            var ammoData = GetNextAmmo();

            UpdateAmmoVisuals();
            
            var bugAddress = "DefaultBug";

            buildingView.Model.SetBuildingHitted(hit, bugAddress, ammoData.Color);

            SetCooldown(View.FireCooldown);
        }

        private void RemoveCurrentAmmo()
        {
            if (!IsReady)
            {
                _inputPlayerModel.ResetReloadButtonValue();
                return;
            }

            _inputPlayerModel.ResetReloadButtonValue();

            LogAmmoDebug();

            GetNextAmmo();
            UpdateAmmoVisuals();
            SetCooldown(View.FireCooldown);
        }

        private void InitializeVisualsMap(LevelDescription levelData)
        {
            _visualsMap = new Dictionary<BuildingColors, SpriteColorByBuildingColor>();
            
            if (levelData.SpriteColorByBuildingColor == null) return;

            foreach (var item in levelData.SpriteColorByBuildingColor)
            {
                _visualsMap.Add(item.BuildingColor, item);
            }
        }
        
        private void UpdateAmmoVisuals()
        {
            var currentAmmo = PeekNextAmmo(0); 
            var nextAmmo = PeekNextAmmo(1);

            _visualsMap.TryGetValue(currentAmmo.Color, out var currentVisual);
            _visualsMap.TryGetValue(nextAmmo.Color, out var nextVisual);

            ApplyVisualToRenderer(View.CurrentBugSprite, currentVisual);
            ApplyVisualToRenderer(View.NextBugSprite, nextVisual);
        }
        
        private void ApplyVisualToRenderer(SpriteRenderer renderer, SpriteColorByBuildingColor data)
        {
            renderer.material.SetTexture("_TintGradient", data.SpriteColorValue);
        }
        
        private void LogAmmoDebug()
        {
            var realCount = AmmoQueue.Count;

            var currentAmmo = PeekNextAmmo(0);
            var nextAmmo = PeekNextAmmo(1);

            var currentTag = (0 >= realCount) ? "[INF]" : "[REAL]";
            var nextTag = (1 >= realCount) ? "[INF]" : "[REAL]";

            var currentInfo = $"{currentTag} {currentAmmo.Color}";
            var nextInfo = $"{nextTag} {nextAmmo.Color}";

            var countInfo = realCount == 0 ? "БЕСКОНЕЧНЫЕ" : realCount.ToString();
            var readyStatus = IsReady ? "ГОТОВ" : $"КД {CurrentCooldown:F1}s";

            Debug.LogWarning($"[Cannon] {readyStatus} | Запас: {countInfo} || Очередь: {currentInfo} -> {nextInfo}");
        }

        private void LoadLevelAmmo(LevelDescription levelDescription, string defaultBugAddress)
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

        private AmmoData GetNextAmmo()
        {
            if (AmmoQueue.Count == 0)
            {
                AddRandomAmmoToQueue();
            }

            return AmmoQueue.Dequeue();
        }

        private AmmoData PeekNextAmmo(int offset = 0)
        {
            while (AmmoQueue.Count <= offset)
            {
                AddRandomAmmoToQueue();
            }

            return AmmoQueue.ElementAt(offset);
        }

        private void UpdateCooldown(float deltaTime)
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= deltaTime;
            }
        }

        private void SetCooldown(float duration)
        {
            CurrentCooldown = duration;
        }
    }
}