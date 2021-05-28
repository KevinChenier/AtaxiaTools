using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerToNose : Tool
{
    private FindRandomPoint randomPoint;

    // Start is called before the first frame update
    void Start()
    {
        randomPoint = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();
        Debug.Log("Test0");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test2");
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            Debug.Log("Test1");
            ToolsManager.Instance.nose.GetComponent<MeshRenderer>().enabled = true;
            ToolsManager.Instance.nose.GetComponent<SphereCollider>().enabled = true;
            gameObject.transform.position = randomPoint.CalculateRandomPoint();
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
