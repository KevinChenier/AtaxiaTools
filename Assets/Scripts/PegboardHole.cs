using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardHole : MonoBehaviour
{
    public Light lightHole; 

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "CylinderPeg")
        {
            lightHole.enabled = true;
            CancelInvoke("CloseLight");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CylinderPeg")
        {
            Invoke("CloseLight", 0.4f);
        }
    }

    void CloseLight()
    {
        lightHole.enabled = false;
    }
}
