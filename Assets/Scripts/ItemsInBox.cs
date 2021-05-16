using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInBox : Tool
{
    public List<Collider> toolObjects;
    public TextMesh toolText;

    private int numberOfCurrentObjects = 0;
    private int numberOfObjects = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (toolObjects.Contains(other))
        {
            numberOfCurrentObjects++;

            if (numberOfCurrentObjects >= numberOfObjects)
                toolText.text = "Test Succeeded!";
            else
                toolText.text = numberOfCurrentObjects.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        toolText.text = "0";
        numberOfObjects = toolObjects.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
