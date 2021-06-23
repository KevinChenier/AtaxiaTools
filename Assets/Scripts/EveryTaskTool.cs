using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EveryTaskTool : Tool<EverydayTaskConfig>
{
    public Text scoreText;
    public GameObject container;
    public Object liquid;
    public float liquidSize;

    public EveryTaskTool() : base("everydayTask") { }

    public override int score()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitTool()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        var spawnPoint = container.transform.FindChildRecursive("SpawnLiquid");

        for (int i = 0; i < base.configs.nbSpheres; i++) 
        {
            GameObject sphere = Instantiate(liquid, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity) as GameObject;
            sphere.transform.parent = GameObject.Find("LiquidParent").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
