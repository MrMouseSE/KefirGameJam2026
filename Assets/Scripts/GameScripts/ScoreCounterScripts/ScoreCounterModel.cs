using GameScripts.InputPlayerSystemScript;
using UnityEngine;

namespace GameScripts.ScoreCounterScripts
{
    public class ScoreCounterModel
    {
        public ScoreCounterSystem System;
        public ScoreCounterView View;

        private int _currentScore;
        private int _currentHealth;
        private InputPlayerSystem _inputSystem;
        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
        
        public void ChangeHealth(int newHealth)
        {
            PlayerPrefs.SetInt("currentScore", newHealth);
        }
        
        public void InitModel(ScoreCounterSystem system, ScoreCounterView view)
        {
            System = system;
            View = view;
            
            _currentScore = PlayerPrefs.GetInt("currentScore", 0);
            _currentHealth = PlayerPrefs.GetInt("currentHealth", 10);
        }

        public void InitSystems(GameSystemsHandler context)
        {
            _inputSystem = context.GetGameSystemByType(typeof(InputPlayerSystem)) as InputPlayerSystem;
        }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            View.ScoreText.text = _currentScore.ToString();
            foreach (var value in context.CurrentDestroyedBuildings.DestroyedBuildingsValues)
            {
                _currentScore += value;
            }

            if (_currentHealth == 0)
            {
                _inputSystem.Model.DisableInput();
            }
            
            context.CurrentDestroyedBuildings.DestroyedBuildingsValues.Clear();
            PlayerPrefs.SetInt("currentScore", _currentScore);
        }
    }
}