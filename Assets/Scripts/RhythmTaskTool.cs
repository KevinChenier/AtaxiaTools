using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTaskTool : Tool<RhythmTaskConfig>
{
    bool active = false;
    GameObject note;
    public AudioClip strumming;
    public Text scoreText;
    public Text clickText;
    int scoreValue;
    int clickValue = 0;

    private static readonly int _baseColor = Shader.PropertyToID("_BaseColor");

    public Color highlightColor = Color.green;
    public float animationTime = 0.01f;

    private Renderer _renderer;
    private Color _originalColor;
    private Color _targetColor;

    public RhythmTaskTool() : base("RhythmTask") { }

    protected override void InitTool()
    {
        scoreValue = 0;

        scoreText.text = "Score : 0 / " + base.configs.nbNotes;

        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            clickValue++;
            clickText.text = "Clicks : " + clickValue;
            if (active)
            {
                Destroy(note);
                _targetColor = highlightColor;
                AudioSource.PlayClipAtPoint(strumming, transform.position, 1);
                active = false;
                AddScore();
            }
        }
        if (_renderer != null)
        {
            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(_baseColor)) // new rendering pipeline (lightweight, hd, universal...)
            {
                _renderer.material.SetColor(_baseColor, Color.Lerp(_renderer.material.GetColor(_baseColor), _targetColor, Time.deltaTime * (1 / animationTime)));

                if (_renderer.material.GetColor(_baseColor) == highlightColor)
                {
                    _targetColor = _originalColor;
                }
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / animationTime));

                if (_renderer.material.color == highlightColor)
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
        Destroy(note);
    }

    void AddScore() 
    {
        scoreValue++;

        scoreText.text = "Score : " + scoreValue + " / " + base.configs.nbNotes;
        Debug.Log(scoreValue);
    }

    public override int score()
    {
        throw new System.NotImplementedException();
    }
}
