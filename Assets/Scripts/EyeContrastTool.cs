using Assets.Scripts.Model;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyeContrastTool : Tool<EyeContrastConfig>
{
    public GameObject Letters;
    public GameObject Options;
    private bool optionsActivated;

    public EyeContrastTool() : base("eyeContrast") { }

    [Range(0.0f, 1.0f)]
    public float contrast;

    private int repetition;
    private float timer;
    private bool success;

    private Transform chosenLetter;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (toolBegan && !optionsActivated)
        {
            if (timer >= 0.06f && !toolEnded)
            {
                contrast -= 0.0001f;

                foreach (Transform child in chosenLetter)
                {
                    child.GetComponent<Renderer>().material.color = Color.HSVToRGB(0.0f, 0.0f, contrast);
                }
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    public void checkChosenLetterValidity()
    {
        success =  EventSystem.current.currentSelectedGameObject.name == chosenLetter.name;
        Debug.Log(success);
        score();
        DeactivateOptions(this, null);
        resetLetter();

        if(success)
            repetition++;

        if (repetition >= configs.repetitions)
        {
            EndTool(5);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    private Transform getRandomLetter()
    {
        System.Random r = new System.Random();
        int i = r.Next(0, Letters.transform.childCount);
        return Letters.transform.GetChild(i);
    }

    private void resetLetter()
    {
        if (chosenLetter == null)
            return;

        contrast = 1.0f;
        foreach (Transform child in chosenLetter)
        {
            child.GetComponent<Renderer>().material.color = Color.HSVToRGB(0.0f, 0.0f, contrast);
            child.GetComponent<MeshRenderer>().enabled = false;
        }

        chosenLetter = getRandomLetter();
        foreach (Transform child in chosenLetter)
        {
            child.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeContrastConfig, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeContrastConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            Repetitions = configs.repetitions
        });
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeContrastData, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeContrastData.ToString(),

            Contrast = contrast,
            Letter = chosenLetter.name,
            Success = success
        });
    }

    public void ActivateOptions(object source, EventArgs args)
    {
        pointer.SetActive(true);
        Options.SetActive(true);
        optionsActivated = true;
        ControllerInputEvent.Instance.TriggerEvent -= ActivateOptions;
    }

    public void DeactivateOptions(object source, EventArgs args)
    {
        pointer.SetActive(false);
        Options.SetActive(false);
        optionsActivated = false;
        ControllerInputEvent.Instance.TriggerEvent += ActivateOptions;
    }

    public override void EndTool(int timer)
    {
        base.EndTool(timer);
    }

    public override void InitTool()
    {
        base.InitTool();
        DeactivateOptions(null, null);
        contrast = 1.0f;
        chosenLetter = getRandomLetter();
        foreach (Transform child in chosenLetter)
        {
            child.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
