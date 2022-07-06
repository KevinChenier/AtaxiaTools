using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioManager
{
    public HashSet<string> toolsDone = new HashSet<string>();
    private List<string> toolsOrder = new List<string>();

    public ScenarioManager(List<string> toolsOrder)
    {
        this.toolsOrder = toolsOrder;
    }

    public void InitScenario()
    {
        SceneManager.LoadScene(toolsOrder[0]);
    }

    public void LoadNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int index = toolsOrder.IndexOf(currentScene);
        toolsDone.Add(currentScene);

        if (index == toolsOrder.Count - 1)
        {
            return;
        }
        else if(index ==  -1)
        {
            SceneManager.LoadScene(toolsOrder[0]);
        }
        else
        {
            SceneManager.LoadScene(toolsOrder[index + 1]);
        }
    }
}
