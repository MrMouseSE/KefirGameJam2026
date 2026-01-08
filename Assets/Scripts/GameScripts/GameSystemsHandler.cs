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
using GameScripts.ScoreCounterScripts;
using UnityEngine;

namespace GameScripts
{
    public class GameSystemsHandler : MonoBehaviour
    {
        public LevelsDescriptionsHolder LevelsDescriptionsHolder;
        public BuildingsPlacerView PlacerView;
        public ScoreCounterView ScoreCounterView;
        public CannonView CannonView;
        
        [Space]
        public ChangeLevelHandler ChangeLevelHandler;

        [HideInInspector]
        public LevelDescription CurrentLevelDescription { get; private set; }

        [HideInInspector]
        public DestroyedBuildings CurrentDestroyedBuildings;

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
            CurrentDestroyedBuildings = new DestroyedBuildings();
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            CurrentLevelDescription = LevelsDescriptionsHolder.LevelDescription.Find(x => x.Level == _currentLevel);
            BuildingStaticFactory.SetLevelDescription(CurrentLevelDescription);
            _gameSystems = new List<IGameSystem>();
            _gameSystems.Add(new BuildingsSpawnerSystem(new BuildingsSpawnerModel(), CurrentLevelDescription));
            _gameSystems.Add(new BuildingsPlacerSystem(new BuildingsPlacerModel(), PlacerView,
                CurrentLevelDescription));
            _gameSystems.Add(new InputPlayerSystem(this, new InputPlayerModel(), Camera.main));
            _gameSystems.Add(new BugSpawnSystem(new BugSpawnModel()));
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

        private void RemoveBuilding()
        {
            BuildingsPlacerSystem placerSystem =
                (BuildingsPlacerSystem)GetGameSystemByType(typeof(BuildingsPlacerSystem));
            foreach (var gameSystem in _systemsToRemove.Where(gameSystem => gameSystem.GetType() == typeof(BuildingSystem)))
            {
                placerSystem.Model.RemoveBuilding((BuildingSystem)gameSystem);
            }
        }

        private void InitializeBuildings()
        {
            BuildingsPlacerSystem placerSystem =
                (BuildingsPlacerSystem)GetGameSystemByType(typeof(BuildingsPlacerSystem));

            foreach (var gameSystem in _buildings)
            {
                gameSystem.InitSystem(this);
                _gameSystems.Add(gameSystem);
                placerSystem.Model.AddBuilding((BuildingSystem)gameSystem);
            }

            _buildings.Clear();
        }
    }
}