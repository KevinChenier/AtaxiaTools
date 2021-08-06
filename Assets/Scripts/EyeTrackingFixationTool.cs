using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using Tobii.G2OM;

public class EyeTrackingFixationTool : Tool<EyeTrackingFixationConfig>, IGazeFocusable
{
    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
    public Color highlightColor = Color.red;
    public float animationTime = 0.1f;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;

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
    }

    public override int score()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitTool()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
        transform.localScale *= (float) base.configs.targetSize;
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
    }
}
