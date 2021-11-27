using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardVictory : MonoBehaviour
{

    public List<GameObject> peggle;
    public GameObject cube;
 
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (peggle.Contains(other.gameObject))
        {
            cube.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
