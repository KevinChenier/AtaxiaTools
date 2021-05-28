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
    }

    void OnTriggerEnter(Collider other)
    {
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            ToolsManager.Instance.nose.GetComponent<MeshRenderer>().enabled = true;
            ToolsManager.Instance.nose.GetComponent<SphereCollider>().enabled = true;
            gameObject.transform.position = randomPoint.CalculateRandomPoint();
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
