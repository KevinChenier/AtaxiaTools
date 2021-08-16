using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;
using UnityEngine.UI;

public class EyeTrackingMultipleGazeAt : MonoBehaviour, IGazeFocusable
{
    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");
    public Color highlightColor = Color.red;
    public float animationTime = 0.1f;

    public Renderer _renderer;
    public Color _originalColor;
    public Color _targetColor;
    public Image _uiFill;

    public bool currentObjectToGazeAt = false;
    public EyeTrackingMultipleTool eyeTrackingMultipleTool;

    private bool hasFocus;
    private float timer;

    public void GazeFocusChanged(bool hasFocus)
    {
        this.hasFocus = hasFocus;
    }

    private void ChangeObjectToGazeAt()
    {
        _targetColor = _originalColor;
        _uiFill.fillAmount = 0;
        currentObjectToGazeAt = false;
        timer = 0;

        int randomObjectIndex = Random.Range(0, eyeTrackingMultipleTool.eyeTrackingMultipleObjects.Count - 1);
        GameObject randomObject = eyeTrackingMultipleTool.eyeTrackingMultipleObjects[randomObjectIndex];
        randomObject.GetComponent<EyeTrackingMultipleGazeAt>()._targetColor = highlightColor;
        randomObject.GetComponent<EyeTrackingMultipleGazeAt>().currentObjectToGazeAt = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
        _uiFill = GetComponentInChildren<Image>();
        _uiFill.fillAmount = 0;
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

        if (hasFocus && currentObjectToGazeAt)
        {
            timer += Time.deltaTime;
            _uiFill.fillAmount = (float)(timer / eyeTrackingMultipleTool.timer);

            if (timer > eyeTrackingMultipleTool.timer)
            {
                ChangeObjectToGazeAt();
            }
        }
    }
}
