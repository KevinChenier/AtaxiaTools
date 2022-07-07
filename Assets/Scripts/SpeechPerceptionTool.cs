using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpeechPerceptionTool : Tool<SpeechPerceptionConfig>
{
    public SpeechPerceptionTool() : base("speechPerception") { }

    public List<AudioClip> letters = new List<AudioClip>();
    private AudioClip chosenLetter;

    private int volumeIncrease;
    private int repetition;
    private float timer;
    private bool success;
    private float volume = 0.1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (toolBegan)
        {
            if (IsInvoking("resetLetter"))
                return;

            if (timer >= 4.0f && !toolEnded)
            {
                HandleResetLetter(false);
            }
            timer += Time.deltaTime;
        }
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.SpeechPerceptionConfig, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.SpeechPerceptionConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            Repetitions = configs.repetitionsPerVolume,
            VolumeIncreases = configs.nbVolumeIncreases
        });
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.SpeechPerceptionData, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.SpeechPerceptionData.ToString(),

            Success = success,
            Letter = chosenLetter.name,
            BackgroundVolume = volume
        });
    }

    private AudioClip getRandomLetter()
    {
        System.Random r = new System.Random();
        int i = r.Next(0, letters.Count);
        return letters[i];
    }

    private void resetLetter()
    {
        chosenLetter = getRandomLetter();
        AudioSource.PlayClipAtPoint(chosenLetter, Camera.main.transform.TransformPoint(Vector3.forward * 2.0f));
    }

    public void checkChosenLetterValidity()
    {
        if (IsInvoking("resetLetter"))
            return;

        HandleResetLetter(EventSystem.current.currentSelectedGameObject.name == chosenLetter.name);
    }

    private void HandleResetLetter(bool success)
    {
        timer = 0;
        this.success = success;
        score();
        repetition++;

        if (repetition >= configs.repetitionsPerVolume)
        {
            volume = GetComponent<AudioSource>().volume += (0.9f / (configs.nbVolumeIncreases - 1));
            repetition = 0;
            volumeIncrease++;
        }

        if (volumeIncrease >= configs.nbVolumeIncreases)
        {
            EndTool(5);
            return;
        }
        Invoke("resetLetter", 1.0f);
    }

    public override void InitTool()
    {
        base.InitTool();
        pointer.SetActive(true);
        GetComponent<AudioSource>().volume = 0.1f;
        GetComponent<AudioSource>().Play();
        Invoke("resetLetter", 1.0f);
    }

    public override void EndTool(int timer)
    {
        base.EndTool(timer);
        GetComponent<AudioSource>().Stop();
    }
}
