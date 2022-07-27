using Assets.Scripts.Model;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RhythmTool : Tool<RhythmConfig>
{
    bool active = false;
    GameObject note;
    public AudioClip strumming;
    public AudioClip miss;
    private int noteCount = 0;

    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");


    public Color highlightColor = Color.green;
    public Color missedColor = Color.red;
    private float animationTime = 0.01f;

    private Assets.Scripts.Model.Types.RhythmNote noteType;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;

    public RhythmTool() : base("rhythm") { }

    public override void InitTool()
    {
        base.InitTool();

        ControllerInputEvent.Instance.TriggerEvent += HandleNoteHit;

        animationTime = 0.02f;
        //animationTime = 60.0f / (base.configs.bpm * 16.0f);
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
    }

    private void HandleNoteHit(object source, EventArgs args)
    {
        if (active)
        {
            Destroy(note);
            _targetColor = highlightColor;
            AudioSource.PlayClipAtPoint(strumming, transform.position, 1);
            active = false;

            noteType = Assets.Scripts.Model.Types.RhythmNote.hit;

            HandleEndTool();
        }
        else
        {
            noteType = Assets.Scripts.Model.Types.RhythmNote.spam;
        }
        score();
    }

    protected override void OnToolChanged(Scene current)
    {
        base.OnToolChanged(current);

        ControllerInputEvent.Instance.TriggerEvent -= HandleNoteHit;
    }

    public override void EndTool(int timer)
    {
        base.EndTool(timer);
    }

    private void HandleEndTool()
    {
        switch (base.configs.mode)
        {
            case Assets.Scripts.Model.Types.RhythmMode.Normal:
                if (noteCount >= base.configs.nbNotes)
                    EndTool(5);
                break;
            case Assets.Scripts.Model.Types.RhythmMode.Clinical:
                if (noteCount >= base.configs.repetitions * base.configs.nbNotesPerRepetitions)
                    EndTool(5);
                break;
        }
    }

    // Update is called once per frame
   void Update()
    {        
        if (_renderer != null)
        {
            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(_baseColor)) // new rendering pipeline (lightweight, hd, universal...)
            {
                _renderer.material.SetColor(_baseColor, Color.Lerp(_renderer.material.GetColor(_baseColor), _targetColor, Time.deltaTime * (1 / animationTime)));

                if (_renderer.material.GetColor(_baseColor) == highlightColor || _renderer.material.GetColor(_baseColor) == missedColor)
                {
                    _targetColor = _originalColor;
                }
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));

                if (_renderer.material.color == highlightColor || _renderer.material.color == missedColor)
                {
                    _targetColor = _originalColor;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Note") 
        {
            active = true;
            note = other.gameObject;
            noteCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // This means the user missed the note
        if (other.gameObject.tag == "Note")
        {
            AudioSource.PlayClipAtPoint(miss, transform.position, 1);
            _targetColor = missedColor;
            active = false;
            Destroy(note);
            noteType = Assets.Scripts.Model.Types.RhythmNote.missed;
            score();
            HandleEndTool();
        }
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.RhythmData, new 
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.RhythmData.ToString(),

            NoteType = noteType.ToString()
        });
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.RhythmConfig, new
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.RhythmConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            Mode = configs.mode.ToString(),
                
            ToolEnded = toolEnded,
            BPM = configs.bpm,
            NumberOfNotes = configs.nbNotes,
            Repetitions = configs.repetitions,
            NumberOfNotesPerRepetitions = configs.nbNotesPerRepetitions
        });
    }
}
