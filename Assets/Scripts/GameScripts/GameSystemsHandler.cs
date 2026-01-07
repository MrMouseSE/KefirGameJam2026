using System;
using System.Collections.Generic;
using GameScripts.BugsScripts;
using GameScripts.BuildingsPlacerSystemScripts;
using GameScripts.BuildingsSpawnerSystemScripts;
using GameScripts.Descriptions;
using UnityEngine;

namespace GameScripts
{
    public class GameSystemsHandler : MonoBehaviour
    {
        public LevelsDescriptionsHolder LevelsDescriptionsHolder;
        public BuildingsPlacerView PlacerView;
        
        private int _currentLevel = 0;
        private List<IGameSystem> _gameSystems;
        
        public BugModel CurrentBug;

        public void InitGameSystems()
        {
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            _gameSystems = new List<IGameSystem>();
            BuildingSpawnerModel spawnerModel = new BuildingSpawnerModel();
            spawnerModel.InitializeLevelBuildings(LevelsDescriptionsHolder.LevelDescription.Find(x => x.Level == _currentLevel));
            _gameSystems.Add(new BuildingsSpawnerSystem(new BuildingSpawnerModel()));
            _gameSystems.Add(new BuildingsPlacerSystem(new BuildingsPlacerModel(), PlacerView));
            
        }
        
        private void Update()
        {
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.UpdateSystem(Time.deltaTime, this);
            }
        }

        public void SetNextLevel()
        {
            _currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        }

        public IGameSystem GetGameSystemByType(Type type)
        {
            return _gameSystems.Find(gameSystem => gameSystem.GetType() == type);
        }
    }
}