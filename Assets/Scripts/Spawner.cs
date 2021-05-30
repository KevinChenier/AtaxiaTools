using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Tool<SimpleToolConfig>
{
    public List<GameObject> gameObjects;

    public Spawner() : base("spawner") { }


    public override int score()
    {
        throw new System.NotImplementedException();
    }

    public void Spawn(int index)
    {
        GameObject spawned = Instantiate(gameObjects[index], transform.position, transform.rotation);
        spawned.AddComponent<Rigidbody>().useGravity = true;
        spawned.AddComponent<MeshCollider>().convex = true;
    }

    protected override void InitTool()
    {
        throw new System.NotImplementedException();
    }
}
