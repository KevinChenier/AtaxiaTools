using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            ToolsManager.Instance.indicator.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
