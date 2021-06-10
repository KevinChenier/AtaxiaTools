using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardCollider : MonoBehaviour
{

    public GameObject[] peggles;
    private Vector3[] initialPos;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < peggles.Length; i++)
        {
            initialPos[i] = peggles[i].transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < peggles.Length; i++)
        {
            if (other == peggles[i])
                peggles[i].transform.position = initialPos[i];
        }
    }
}
