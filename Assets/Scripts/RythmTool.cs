using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmTool : MonoBehaviour
{

    bool active = false;
    GameObject note;
    public AudioClip strumming;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) && active)
        {
            Destroy(note);
            active = false;
            AddScore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Note") 
        {
            Debug.Log("Note");
            //AudioSource.PlayClipAtPoint(strumming, transform.position, 1);
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
        score++;
        Debug.Log(score);
    }
}
