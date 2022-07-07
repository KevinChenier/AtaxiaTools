using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ScenarioManager
{
    public HashSet<string> toolsDone = new HashSet<string>();
    private List<string> toolsOrder = new List<string>();
    private int currentToolIndex = 0;

    public ScenarioManager(List<string> toolsOrder)
    {
        this.toolsOrder = toolsOrder;
    }

    public void InitScenario()
    {
        if (PatientData.PatientID == null)
        {
            SceneManager.LoadScene("PatientScene");
        }
        else
        {
            PatientData.TrialID = System.Guid.NewGuid().ToString();
            SceneManager.LoadScene(toolsOrder[0]);
        }
    }

    public void LoadNextScene()
    {
        if (PatientData.PatientID == null)
        {
            Debug.LogError("Need to set Patient ID.");
            return;
        }
            
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
        }
    }
}
