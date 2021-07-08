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
    public EveryTaskTool everydayTool;

    private bool isHeightOk;
    private int nbSpheres;
    private int accuracy;

    private void Start()
    {
        accuracy = 0;
        isHeightOk = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LiquidSphere" && isHeightOk )
        {
            accuracy++;
            Debug.Log(accuracy);
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
