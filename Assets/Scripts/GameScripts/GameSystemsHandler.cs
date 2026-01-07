using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.BugsScripts;
using GameScripts.BuildingFactory;
using GameScripts.BuildingScripts;
using GameScripts.BuildingsPlacerSystemScripts;
using GameScripts.BuildingsSpawnerSystemScripts;
using GameScripts.CannonScripts;
using GameScripts.ChangeLevelScript;
using GameScripts.Descriptions;
using GameScripts.InputPlayerSystemScript;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameScripts
{
    public class GameSystemsHandler : MonoBehaviour
    {
        private const string DEFAULT_BUG_ADDRESS = "DefaultBug";
        
        public LevelsDescriptionsHolder LevelsDescriptionsHolder;
        public BuildingsPlacerView PlacerView;
        public CannonView CannonView;
        
        [Space]
        public ChangeLevelHandler ChangeLevelHandler;

        [HideInInspector] public LevelDescription CurrentLevelDescription { get; private set; }
        
        [HideInInspector] public bool IsBugSpawned;
        [HideInInspector] public BugModel CurrentBug;
        
        private int _currentLevel = 0;
        private List<IGameSystem> _gameSystems;
        
        private bool _isGameRunnign;
        
        private List<IGameSystem> _buildings = new List<IGameSystem>();
        
        private bool _isBuildingSpawned;
        
        private List<IGameSystem> _systemsToRemove = new List<IGameSystem>();
        private List<IGameSystem> _systemsToAdd = new();

        private void Awake()
        {
            InitGameSystems();
        }

        public void InitGameSystems()
        {
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            CurrentLevelDescription = LevelsDescriptionsHolder.LevelDescription.Find(x => x.Level == _currentLevel);
            BuildingStaticFactory.SetLevelDescription(CurrentLevelDescription);
            _gameSystems = new List<IGameSystem>();
            _gameSystems.Add(new BuildingsSpawnerSystem(new BuildingsSpawnerModel(), CurrentLevelDescription));
            _gameSystems.Add(new BuildingsPlacerSystem(new BuildingsPlacerModel(), PlacerView, CurrentLevelDescription));
            _gameSystems.Add(new InputPlayerSystem(new InputPlayerModel(), Camera.main));
            
            _gameSystems.Add(new CannonSystem(new CannonModel(), CannonView));
            
            foreach (var system in _gameSystems)
            {
                system.InitSystem(this);
            }
            
            _isGameRunnign = true;
        }
        
        public void AddBuildingSystem(BuildingSystem buildingSystem, bool isBuildingSpawned)
        {
            _isBuildingSpawned = isBuildingSpawned;
            _buildings.Add(buildingSystem);
        }
        
        public IGameSystem GetGameSystemByType(Type type)
        {
            return _gameSystems.Find(gameSystem => gameSystem.GetType() == type);
        }

        public void CompleteCurrentLevel()
        {
            _isGameRunnign = false;
            _currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
            ChangeLevelHandler.ChangeLevel(this);
        }

        public void AddSystemToDelete(IGameSystem system)
        {
            _systemsToRemove.Add(system);
        }

        public void AddNewSystem(IGameSystem system)
        {
            _systemsToAdd.Add(system);
        }
        
        private void Update()
        {
            if (!_isGameRunnign) return;

            if (_systemsToAdd.Count > 0)
            {
                _gameSystems.AddRange(_systemsToAdd);
                _systemsToAdd.Clear();
            }
            
            if (_isBuildingSpawned) InitializeBuildings();
            
            if (_systemsToRemove.Count > 0)
            {
                _gameSystems = _gameSystems.Except(_systemsToRemove).ToList();
                _systemsToRemove.Clear();
            }
            
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.UpdateSystem(Time.deltaTime, this);
            }
        }

        private void InitializeBuildings()
        {
            BuildingsPlacerSystem placerSystem = (BuildingsPlacerSystem)GetGameSystemByType(typeof(BuildingsPlacerSystem));
            
            foreach (var gameSystem in _buildings)
            {
                gameSystem.InitSystem(this);
                _gameSystems.Add(gameSystem);
                placerSystem.Model.AddBuilding((BuildingSystem)gameSystem);
            }
            _buildings.Clear();
        }
        
        public void CreateAndRegisterBug(string addressKey, Vector3 position, BuildingModel target, BuildingColors assignedColor)
        {
            string keyToLoad = string.IsNullOrEmpty(addressKey) ? DEFAULT_BUG_ADDRESS : addressKey;

            Addressables.InstantiateAsync(keyToLoad, position, Quaternion.identity).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var bugGo = handle.Result;
                    
                    InitializeBugLogic(bugGo, target, assignedColor);
                }
                else
                {
                    Debug.LogError($"Failed to spawn bug from Addressables. Key: {keyToLoad}");
                }
            };
        }
        
        private void InitializeBugLogic(GameObject bugGo, BuildingModel target, BuildingColors assignedColor)
        {
            var bugView = bugGo.GetComponent<BugView>();

            if (CurrentLevelDescription != null)
            {
                var colorData = CurrentLevelDescription.SpriteColorByBuildingColor.Find(x => x.BuildingColor == assignedColor);
                if (colorData != null && bugView != null)
                {
                    bugView.SetColor(assignedColor);
                }
            }

            var bugModel = new BugModel();
            bugModel.TargetBuilding = target;
            bugModel.BuildingColor = assignedColor;

            var bugSystem = new BugSystem(bugModel, bugView);
            bugSystem.InitSystem(this);

            AddNewSystem(bugSystem);

            CurrentBug = bugModel;
            IsBugSpawned = true;
        }

        public void OnBugDied()
        {
            CurrentBug = null;
            IsBugSpawned = false;
        }
    }
}