using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EveryTaskTool : Tool<EverydayTaskConfig>
{
    public GameObject container;
    public Object liquid;
    public float liquidSize;
    public Text scoreText;

    private Transform spawnPoint;

    public EveryTaskTool() : base("EverydayTask") { }

    public override int score()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitTool()
    {
        scoreText.text = "Score : 0 / " + base.configs.nbSpheres;
        spawnPoint = container.transform.FindChildRecursive("SpawnLiquid");
        for (int i = 0; i < base.configs.nbSpheres - 1; i++)
        {
            GameObject sphere = Instantiate(liquid, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity) as GameObject;
            sphere.transform.parent = GameObject.Find("LiquidParent").transform;
            Debug.Log(i);
        }
    }
}
