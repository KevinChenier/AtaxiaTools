using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "LiquidRecipient")
        {
            Debug.Log("Liquid in");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
