using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScenarioManager
{
    public HashSet<string> toolsDone = new HashSet<string>();
    public List<string> toolsOrder = new List<string>();
    private int currentToolIndex = 0;

    public ScenarioManager(List<string> toolsOrder)
    {
        this.toolsOrder = toolsOrder;
    }

    public void InitScenario()
    {
        // Commented until a new solution is working
        /*if (PatientData.PatientID == "Default")
        {
            SceneManager.LoadScene("PatientScene");
        }
        else
        {
            PatientData.TrialID = Guid.NewGuid().ToString();
            SceneManager.LoadScene(toolsOrder[0]);
        }*/
        SceneManager.LoadScene(toolsOrder[0]);
    }

    public void LoadNextScene()
    {
        // Commented until a new solution is working
        /*if (PatientData.PatientID == "Default")
        {
            Debug.LogError("Need to set Patient ID.");
            return;
        }*/

        string currentScene = SceneManager.GetActiveScene().name;
        toolsDone.Add(currentScene);
        try
        {
            currentToolIndex++;
            SceneManager.LoadScene(toolsOrder[currentToolIndex]);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("Finished Scenario!");
            Application.Quit();
        }
    }
}
