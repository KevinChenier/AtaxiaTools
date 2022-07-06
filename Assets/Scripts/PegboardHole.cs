using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardHole : MonoBehaviour
{
    public Light lightHole; 

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CylinderPeg")
        {
            lightHole.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CylinderPeg")
        {
            lightHole.enabled = false;
        }
    }
}
