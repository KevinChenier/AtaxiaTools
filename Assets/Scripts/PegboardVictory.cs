using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardVictory : MonoBehaviour
{

    public List<GameObject> peggle;
    public GameObject cube;
 
    // Start is called before the first frame update

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (peggle.Contains(other.gameObject))
        {
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (peggle.Contains(other.gameObject))
        {
            //cube.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
