using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatientHandler : MonoBehaviour
{
    public void SetPatientID(GameObject Text)
    {
        PatientData.PatientID = Text.GetComponent<TMP_Text>().text;

        if(ConfigManager.Instance.Config.ScenarioActive)
            ConfigManager.Instance.ScenarioManager.InitScenario();
    }
}
