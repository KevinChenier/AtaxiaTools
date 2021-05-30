using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Tool
{
    public List<GameObject> gameObjects;

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
}
