using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardHole : MonoBehaviour
{
    public Light lightHole;
    public PegboardTool tool;

    private void Update()
    {
        foreach(Collider peggle in tool.toolObjects)
        {
            if (BoundsIsEncapsulated(GetComponent<MeshCollider>().bounds, peggle.bounds))
            {
                lightHole.enabled = true;
                return;
            }
        }
        lightHole.enabled = false;
    }

    bool BoundsIsEncapsulated(Bounds Encapsulator, Bounds Encapsulating)
    {
        return Encapsulator.Contains(Encapsulating.center) && Encapsulator.Contains(Encapsulating.min) && Encapsulator.Contains(Encapsulating.max);
    }
}
