using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerToNose : Tool<FingerNoseConfig>
{
    private FindRandomPoint randomPoint;

    public FingerToNose() : base("fingerToNose") { }

    // Start is called before the first frame update
    void Start()
    {
        randomPoint = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();
    }
    public void Recalculate()
    {
        randomPoint.Recalculate();
        gameObject.transform.position = randomPoint.CalculateRandomPoint();
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
        throw new System.NotImplementedException();
    }

    public override int score()
    {
        throw new System.NotImplementedException();
    }
}
