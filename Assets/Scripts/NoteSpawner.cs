using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject note;
    public Transform spawn;
    public float beat;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > beat)
        {
            GameObject cube = Instantiate(note, spawn);
            cube.transform.localPosition = Vector3.zero;
            timer -= beat;
        }

        timer += Time.deltaTime;
    }
}
