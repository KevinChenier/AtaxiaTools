using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTool : Tool<RhythmConfig>
{
    bool active = false;
    GameObject note;
    public AudioClip strumming;
    public AudioClip miss;
    private int scoreValue = 0;
    private int clickValue = 0;
    private int noteCount = 0;

    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");

    public Color highlightColor = Color.green;
    public Color missedColor = Color.red;
    private float animationTime = 0.01f;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;

    public RhythmTool() : base("Rhythm") { }

    protected override void InitTool()
    {
        base.InitTool();

        animationTime = 60.0f / (base.configs.bpm * 16.0f);
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
    }

    public override void EndTool(int timer)
    {
        score();
        base.EndTool(timer);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            clickValue++;
            if (active)
            {
                Destroy(note);
                _targetColor = highlightColor;
                AudioSource.PlayClipAtPoint(strumming, transform.position, 1);
                active = false;
                scoreValue++;
            }
        }
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

            if (noteCount >= base.configs.nbNotes)
            {
                EndTool(5);
            }
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
        }
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.RhythmData, new 
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.RhythmData.ToString(), 

            Score = (double) scoreValue / base.configs.nbNotes * 100 + "%", 
            Precision = (double) scoreValue / clickValue * 100 + "%"
        });
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.RhythmConfig, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.RhythmConfig.ToString(),

            ToolEnded = toolEnded,
            BPM = configs.bpm,
            NumberOfNotes = configs.nbNotes
        });
    }
}
