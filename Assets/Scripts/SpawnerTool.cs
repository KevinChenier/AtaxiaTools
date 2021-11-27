using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTool : Tool<SimpleToolConfig>
{
    public List<GameObject> gameObjects;

    public SpawnerTool() : base("spawner") { }

    public override void configsSave()
    {
        throw new System.NotImplementedException();
    }

    public override void score()
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
        base.InitTool();
    }
}
