using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public UnityEngine.Object note;

    public RhythmTool rhythmTool;

    private float timer;
    private float timerBetweenRepetitions;
    private int noteCount = 0;
    private int repetitionsCount = 0;

    public Assets.Scripts.Model.Types.RhythmMode Mode;

    private int currentRandomBpm = 0;
    private List<int> randomBpms;

    void Start()
    {
        Mode = rhythmTool.configs.mode;

        if (Mode == Assets.Scripts.Model.Types.RhythmMode.Clinical)
            FillRandomBpms();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mode)
        {
            case Assets.Scripts.Model.Types.RhythmMode.Normal:
                HandleNormalRhythm();
                break;
            case Assets.Scripts.Model.Types.RhythmMode.Clinical:
                HandleClinicalRhythm();
                break;
        }

           
    }

    void FillRandomBpms()
    {
        randomBpms = new List<int>();
        System.Random r = new System.Random();

        for (int i = 0; i < rhythmTool.configs.nbNotesPerRepetitions; i++)
        {
            int randomBpm = r.Next(90, 200);
            randomBpms.Add(randomBpm);
        }
    }


    void HandleNormalRhythm()
    {
        if (noteCount < rhythmTool.configs.nbNotes)
        {
            if (timer >= (60.0f / rhythmTool.configs.bpm))
            {
                Instantiate(note, transform.position, rhythmTool.transform.rotation, this.transform);
                noteCount++;
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    void HandleClinicalRhythm()
    {
        if (timerBetweenRepetitions >= 0.0f)
        {
            timerBetweenRepetitions -= Time.deltaTime;
        }
        else if (repetitionsCount < rhythmTool.configs.repetitions)
        {
            if (timer >= (60.0f / randomBpms[currentRandomBpm]))
            {
                GameObject noteGameObject = (GameObject) Instantiate(note, transform.position, rhythmTool.transform.rotation, this.transform);

                if ((repetitionsCount >= rhythmTool.configs.repetitions / 2) && (currentRandomBpm > 0))
                    noteGameObject.GetComponent<MeshRenderer>().enabled = false;

                timer = 0;
                currentRandomBpm++;

                // This means we are in a new repetition
                if (currentRandomBpm >= randomBpms.Count)
                {
                    timerBetweenRepetitions = 3.0f;
                    currentRandomBpm = 0;
                    repetitionsCount++;
                }
            }
            timer += Time.deltaTime;
        }
    }
}