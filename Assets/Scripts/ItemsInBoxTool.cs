using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInBox : Tool<SimpleToolConfig>
{
    public List<Collider> toolObjects;
    public TextMesh toolText;

    private int numberOfCurrentObjects = 0;
    private int numberOfObjects = 0;

    public override void InitTool()
    {
        base.InitTool();
    }

    public override void score()
    {
        throw new System.NotImplementedException();
    }

    public ItemsInBox(): base("itemsInBox") { }

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

    public override void configsSave()
    {
        throw new System.NotImplementedException();
    }
}
