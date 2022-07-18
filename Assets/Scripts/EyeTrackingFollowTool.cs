using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using Tobii.G2OM;

public class EyeTrackingFollowTool : Tool<EyeTrackingFollowConfig>, IGazeFocusable
{
    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
    public Color highlightColor = Color.red;
    public float animationTime = 0.1f;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;

    [Range(1.0f, 5.0f)]
    public float speedModifier = 1.0f;

    private Vector3 startPos;
    private Vector3 endPos;

    public GameObject plane;
    private float lerpValue = 0.0f;
    private FindRandomPoint randomPoint;

    private bool hasFocus;

    private int repetitions = 1;

    public EyeTrackingFollowTool() : base("eyeTrackingFollow") { }

    public void GazeFocusChanged(bool hasFocus)
    {
        //If this object received focus, fade the object's color to highlight color
        if (hasFocus)
        {
            _targetColor = highlightColor;
        }
        //If this object lost focus, fade the object's color to it's original color
        else
        {
            _targetColor = _originalColor;
        }
        this.hasFocus = hasFocus;
    }

    void OnEnable()
    {
        plane.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void InitTool()
    {
        base.InitTool();

        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
        gameObject.transform.parent.localScale *= (float)base.configs.targetSize;

        randomPoint = plane.GetComponent<FindRandomPoint>();
        startPos = randomPoint.CalculateRandomPoint();
        endPos = randomPoint.CalculateRandomPoint();
        speedModifier = (float)base.configs.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_renderer != null)
        {
            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(_baseColor))
            {
                _renderer.material.SetColor(_baseColor, Color.Lerp(_renderer.material.GetColor(_baseColor), _targetColor, Time.deltaTime * (1 / animationTime)));
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));
            }
        }

        if (hasFocus && randomPoint != null)
        {
            if (gameObject.transform.position == endPos)
            {
                repetitions++;

                if (repetitions >= base.configs.repetitions)
                {
                    EndTool(5);
                }
                else
                {
                    startPos = gameObject.transform.position;
                    endPos = randomPoint.CalculateRandomPoint();

                    lerpValue = 0;
                }
            }
            AdvanceCube();
        }
    }

    void AdvanceCube()
    {
        lerpValue += Time.deltaTime;
        gameObject.transform.position = Vector3.Lerp(startPos, endPos, (lerpValue * speedModifier) / Vector3.Distance(startPos, endPos));
    }

    public override void score()
    {
        throw new System.NotImplementedException();
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeTrackingFollowConfig, new
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeTrackingFollowConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            Repetitions = configs.repetitions,
            TargetSpeed = configs.speed,
            TargetSize = configs.targetSize
        });
    }
}