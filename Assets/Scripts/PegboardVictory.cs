using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardVictory : MonoBehaviour
{

    public GameObject peggle;
    public GameObject cube;
    // Start is called before the first frame update

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == peggle)
        {
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == peggle)
        {
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
