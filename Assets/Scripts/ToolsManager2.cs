using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ToolsManager2 : MonoBehaviour
    {
        public Tool tool;
        public GameObject menu;

        private Button[] buttons;
        private string sceneName;
        private Canvas canvas;
        private static HashSet<string> possibleSceneNames;
        private bool IsMenu = true;
        
        void Awake()
        {
            if (possibleSceneNames == null)
            {
                InitPossibleScenes();
            }
            sceneName = SceneManager.GetActiveScene().name;
            canvas = menu.GetComponent<Canvas>();
            buttons = menu.GetComponentsInChildren<Button>();
            if (tool != null)
            {
                IsMenu = false;
                tool.Show();
                HideCanvas();
            }
            foreach (var b in buttons)
            {
                SetOnClick(b);
            }
        }

        private void InitPossibleScenes()
        {
            int count = SceneManager.sceneCountInBuildSettings;
            possibleSceneNames = new HashSet<string>();
            for(var i = 0; i < count; i++)
            {
                possibleSceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }

        void Update()
        {
            if (IsMenu)
            {
                ShowCanvas();
            } 
            else
            {
                HideCanvas();
            }

            if (tool != null && OVRInput.GetUp(OVRInput.Button.Start))
            {
                
                if (IsMenu)
                {
                    tool.Show();
                    ShowCanvas();
                    IsMenu = false;
                } 
                else
                {
                    tool.Pause();
                    HideCanvas();
                    IsMenu = true;
                }
            }
        }

        void SetOnClick(Button b)
        {
            var sceneName = $"{b.name.Substring(14)}Scene";
            if (this.sceneName == sceneName || !possibleSceneNames.Contains(sceneName))
            {
                Debug.Log(sceneName + " is an invalid scene name");
                b.enabled = false;
            }
            else
            {
                Debug.Log(sceneName + " is a valid scene name");
                b.onClick.AddListener(() => GoTo(sceneName));
            }
        }

        void GoTo(string sceneName)
        {
            StartCoroutine(LoadYourAsyncScene(sceneName));
        }

        IEnumerator LoadYourAsyncScene(string name)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        void ShowCanvas() 
        {
            canvas.gameObject.SetActive(true);
        }

        void HideCanvas() 
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
