using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerToNoseTool : Tool<FingerNoseConfig>
{
    private FindRandomPoint randomPoint;

    public FingerToNoseTool() : base("fingerToNose") { }

    public void Recalculate()
    {
        //randomPoint.Recalculate();
        gameObject.transform.position = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>().CalculateRandomPoint();
    }
    void OnTriggerEnter(Collider other)
    {
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            ToolsManager.Instance.nose.GetComponent<MeshRenderer>().enabled = true;
            ToolsManager.Instance.nose.GetComponent<SphereCollider>().enabled = true;
            Recalculate();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }

    protected override void InitTool()
    {
        randomPoint = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();
    }

    public override int score()
    {
        throw new System.NotImplementedException();
    }
}
