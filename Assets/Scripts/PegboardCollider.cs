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

        for (int i = 0; i < peggles.Count; i++)
        {
            peggles[i] = new GameObject();
            InitialPos[i] = peggles[i].transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < peggles.Count; i++)
        {
            if (other == peggles[i])
            {
                peggles[i].transform.position = InitialPos[i];
                Debug.Log("" + peggles[i].transform.position);
            }
        }
    }
}
