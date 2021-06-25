using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardCollider : MonoBehaviour
{

    public List<GameObject> peggles;
    private List<Vector3> InitialPos;

    // Start is called before the first frame update
    void Start()
    {
        InitialPos = new List<Vector3>();

        for (int i = 0; i < peggles.Count; i++)
        {
            Debug.Log("" + peggles[i].transform.position);
            InitialPos.Add(peggles[i].transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("" + other.name);
        for (int i = 0; i < peggles.Count; i++)
        {
            if (other.gameObject == peggles[i])
            {
                peggles[i].transform.position = InitialPos[i];
                Debug.Log("" + peggles[i].transform.position);
            }
        }
    }
}
