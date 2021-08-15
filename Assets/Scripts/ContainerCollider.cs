using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerCollider : MonoBehaviour
{
    public GameObject PourLine;
    public GameObject Container;
    public GameObject Recipient;
    public Text scoreText;
    public EverydayTaskTool everydayTool;

    private bool isHeightOk;
    private int accuracy;

    private void Start()
    {
        accuracy = 0;
        isHeightOk = false;
        PourLine.transform.position = new Vector3(PourLine.transform.position.x, everydayTool.configs.height, PourLine.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LiquidSphere" && isHeightOk )
        {
            accuracy++;
            scoreText.text = "Score : " + accuracy + " / " + everydayTool.configs.nbSpheres;
        }  
    }

    private void Update()
    {
        if (Container.transform.position.y >= PourLine.transform.position.y)
        {
            isHeightOk = true;
        }
        else
        {
            isHeightOk = false;
        }
    }
}
