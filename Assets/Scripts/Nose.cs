using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nose : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (AvatarManager.Instance.indexes_hand.Contains(other))
        {
            AvatarManager.Instance.indicator.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
