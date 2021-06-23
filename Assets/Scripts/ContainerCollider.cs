using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCollider : MonoBehaviour
{
    public GameObject PourLine;

    private bool isHeightOk;
    private int nbSpheres;

    // Start is called before the first frame update
    void Awake()
    {
        nbSpheres = ConfigManager.Instance.GetToolConfig<EverydayTaskConfig>("EverydayTask").nbSpheres;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "LiquidRecipient" && isHeightOk)
        {
            Debug.Log("Liquid in");

            /*
             var++
             score.text = "Score : " + var + " / " + nbSpheres
             
             */
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LiquidContainer")
        {
            if (other.gameObject.transform.position.y >= PourLine.transform.position.y)
            {
                isHeightOk = true;
            } 
            else
            {
                isHeightOk = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
