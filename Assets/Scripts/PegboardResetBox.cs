using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PegboardResetBox : MonoBehaviour
{
    private int scoreBox;
    private bool isPressed = false;

    public List<GameObject> lights;
    public List<GameObject> peggle;
    public GameObject cube;
    public TextMesh toolText;
    private HashSet<Collider> colliders = new HashSet<Collider>();

    public PegboardTool pegboardTool;

    private void OnTriggerEnter(Collider other)
    {
        if (peggle.Contains(other.gameObject))
        {
            scoreBox = 0;
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].GetComponent<MeshRenderer>().enabled)
                {
                    scoreBox++;
                }
            }

            if (scoreBox == 9)
            {
                colliders.Add(other);
            }

            if (colliders.Count == 9)
            {
                cube.GetComponent<MeshRenderer>().enabled = true;
                pegboardTool.EndTool(5);
            }
        }
    }
}