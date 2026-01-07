using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScripts
{
    public class StartButtonHandler : MonoBehaviour
    {
        public Button ButtonObject;
        public AssetReference SceneAsset;

        private void Awake()
        {
            ButtonObject.onClick.AddListener(StartMethod);
        }

        private void StartMethod()
        {
            SceneAsset.LoadSceneAsync(LoadSceneMode.Additive);
        }
    }
}