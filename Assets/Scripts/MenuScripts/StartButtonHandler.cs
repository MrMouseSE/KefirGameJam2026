using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScripts
{
    public class StartButtonHandler : MonoBehaviour
    {
        public Button ButtonObject;
        public AssetReference SceneAsset;
        
        public Action OnSceneLoaded; 

        private void Awake()
        {
            ButtonObject.onClick.AddListener(StartMethod);
        }

        private void StartMethod()
        {
            var handle = SceneAsset.LoadSceneAsync(LoadSceneMode.Additive);
            handle.Completed += SceneLoaded;
        }

        private void SceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            SceneManager.SetActiveScene(obj.Result.Scene);
            OnSceneLoaded?.Invoke();
        }
    }
}