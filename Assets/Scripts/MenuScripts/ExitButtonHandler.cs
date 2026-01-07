using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    public class ExitButtonHandler : MonoBehaviour
    {
        public Button ButtonObject;

        private void Awake()
        {
            ButtonObject.onClick.AddListener(ExitMethod);
        }

        private void ExitMethod()
        {
            ButtonObject.onClick.RemoveAllListeners();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
