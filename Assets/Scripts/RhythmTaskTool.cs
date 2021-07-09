using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTaskTool : Tool<RhythmTaskConfig>
{
    bool active = false;
    GameObject note;
    public AudioClip strumming;
    public Text scoreText;
    int scoreValue;

    public RhythmTaskTool() : base("RhythmTask") { }

    protected override void InitTool()
    {
        scoreValue = 0;

        scoreText.text = "Score : 0 / " + base.configs.nbNotes;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && active)
        {
            Destroy(note);
            AudioSource.PlayClipAtPoint(strumming, transform.position, 1);
            active = false;
            AddScore();
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
