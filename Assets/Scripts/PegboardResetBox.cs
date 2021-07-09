using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PegboardResetBox : MonoBehaviour
{
    private int scoreBox;
    private bool isPressed = false;
    private float timeGrab = 0;

    public List<GameObject> lights;
    public List<GameObject> peggle;
    public GameObject cube;
    public TextMesh toolText;
    private HashSet<Collider> colliders = new HashSet<Collider>();

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || isPressed)
        {
            timeGrab += Time.deltaTime;
            isPressed = true;
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            isPressed = false;
            toolText.text = "" + timeGrab;
            //timeGrab = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (peggle.Contains(other.gameObject))
        {
            scoreBox = 0;
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].GetComponent<MeshRenderer>().enabled) 
                {
                    scoreBox++;
                }
            }

            if(scoreBox == 9)
            {
                colliders.Add(other);
            }

            if(colliders.Count == 9) 
            {
                cube.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
