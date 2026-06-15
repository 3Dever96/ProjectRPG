using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectRPG;

namespace ProjectRPG.Managers
{
    public class SceneHub : MonoBehaviour
    {
        public static SceneHub Instance {  get; private set; }

        public List<SceneData> defaultScenes = new List<SceneData>();
        public List<SceneData> currentScenes = new List<SceneData>();

        private void Awake()
        {
            Instance = this;

            ChangeScenes(null, defaultScenes);
        }

        public void ChangeScenes(List<SceneData> scenesToUnload, List<SceneData> scenesToLoad)
        {
            StartCoroutine(SceneTransition(scenesToUnload, scenesToLoad));
        }

        public IEnumerator SceneTransition(List<SceneData> scenesToUnload, List<SceneData> scenesToLoad)
        {
            AsyncOperation operation = new AsyncOperation();

            if (scenesToUnload != null)
            {
                for (var i = scenesToUnload.Count - 1; i >= 0; i--)
                {
                    if (currentScenes.Contains(scenesToUnload[i]))
                    {
                        operation = SceneManager.UnloadSceneAsync(scenesToUnload[i].sceneName);
                        currentScenes.RemoveAt(i);
                    }
                }
            }

            yield return null;

            if (scenesToLoad != null)
            {
                for (var i = 0; i < scenesToLoad.Count; i++)
                {
                    if (!currentScenes.Contains(scenesToLoad[i]))
                    {
                        SceneManager.LoadSceneAsync(scenesToLoad[i].sceneName, scenesToLoad[i].loadMode);
                        currentScenes.Add(scenesToLoad[i]);
                    }
                }
            }
        }
    }
}
