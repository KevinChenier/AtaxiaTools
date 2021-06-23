using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EveryTaskTool : Tool<EverydayTaskConfig>
{
    public int nbSpheres = 100;
    public Text scoreText;

    public GameObject container;
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

        for(int i = 0; i < nbSpheres; i++) 
        {
            
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            //GameObject sphere = Instantiate(magicPickup, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity) as GameObject;
            sphere.transform.localScale = new Vector3(liquidSize, liquidSize, liquidSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
