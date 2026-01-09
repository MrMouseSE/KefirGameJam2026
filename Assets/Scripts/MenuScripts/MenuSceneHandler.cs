using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class MenuSceneHandler : MonoBehaviour
    {
        public Camera MainCamera;
        
        public StartButtonHandler StartButton;

        public GameObject[] MenuSceneObjects;

        private void Awake()
        {
            StartButton.OnSceneLoaded += DeactivateMenuScene;
        }

        public void DeactivateMenuScene()
        {
            ActivateMethod(false);
        }
        
        public void ActivateMenuScene()
        {
            SceneManager.SetActiveScene(gameObject.scene);
            ActivateMethod(true);
        }

        private void ActivateMethod(bool isActivate)
        {
            foreach (var menuSceneObject in MenuSceneObjects)
            {
                menuSceneObject.SetActive(isActivate);
            }
        }
    }
}
