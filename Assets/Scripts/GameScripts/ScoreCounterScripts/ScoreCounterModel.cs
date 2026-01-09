using GameScripts.InputPlayerSystemScript;
using UnityEngine;

namespace GameScripts.ScoreCounterScripts
{
    public class ScoreCounterModel
    {
        public ScoreCounterSystem System;
        public ScoreCounterView View;

        private int _currentScore;

        public void InitModel(ScoreCounterSystem system, ScoreCounterView view)
        {
            System = system;
            View = view;
            
            _currentScore = PlayerPrefs.GetInt("currentScore", 0);
        }

        public void InitSystems(GameSystemsHandler context) { }
        
        public void UpdateModel(float deltaTime, GameSystemsHandler context)
        {
            View.ScoreText.text = _currentScore.ToString();
            foreach (var value in context.CurrentDestroyedBuildings.DestroyedBuildingsValues)
            {
                _currentScore += value;
            }
            
            context.CurrentDestroyedBuildings.DestroyedBuildingsValues.Clear();
            PlayerPrefs.SetInt("currentScore", _currentScore);
        }
    }
}