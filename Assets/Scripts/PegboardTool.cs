using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardTool : Tool<PegboardConfig>
{
    public List<Collider> toolObjects;
    public TextMesh toolText;

    private int numberOfCurrentObjects = 0;
    private int numberOfObjects = 0;

    public PegboardTool() : base("pegboard") { }

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

    public override int score()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitTool()
    {
        toolText.text = "0";
        numberOfObjects = toolObjects.Count;

        //throw new System.NotImplementedException();
    }
}
