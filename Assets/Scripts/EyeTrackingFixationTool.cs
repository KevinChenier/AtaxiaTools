using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using Tobii.G2OM;
using UnityEngine.UI;

public class EyeTrackingFixationTool : Tool<EyeTrackingFixationConfig>, IGazeFocusable
{
    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
    public Color highlightColor = Color.red;
    public float animationTime = 0.1f;

    private float timeFixation;
    private bool hasFocus;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;
    public Image _uiFill { get; set; }

    public EyeTrackingFixationTool() : base("eyeTrackingFixation") { }

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

    public override void score()
    {
        throw new System.NotImplementedException();
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeTrackingFixConfig, new
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeTrackingFixConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            TargetSize = configs.targetSize,
            TimeFixation = configs.timeFixation,
            Distance = configs.distance
        });
    }

    public override void InitTool()
    {
        base.InitTool();

        calibrateTarget();

        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
        transform.localScale *= (float) base.configs.targetSize;
        _uiFill = GetComponentInChildren<Image>();
        _uiFill.fillAmount = 0;
    }

    private void calibrateTarget()
    {
        transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        Vector3 direction = transform.position - Camera.main.transform.position;

        Debug.Log(Camera.main.transform.position);

        direction = direction.normalized;
        transform.position = Camera.main.transform.position;
        transform.Translate(direction * (float)base.configs.distance);
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
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));
            }
        }

        if(hasFocus && _uiFill != null)
        {
            timeFixation += Time.deltaTime;
            _uiFill.fillAmount = (float)(timeFixation / base.configs.timeFixation);
        }

        if (timeFixation >= (float)base.configs.timeFixation)
        {
            EndTool(5);
            timeFixation = Mathf.NegativeInfinity;
        }
    }
}
