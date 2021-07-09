using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public Object note;
    public Transform spawn;
    public float beat;

    public RhythmTaskTool rhythmTool;

    private float timer;
    private int noteCount;

    // Start is called before the first frame update
    void Start()
    {
        noteCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (noteCount < rhythmTool.configs.nbNotes) 
        {
            if (timer > beat)
            {
                
                GameObject cube = Instantiate(note, spawn.position, Quaternion.identity) as GameObject;
                cube.transform.eulerAngles = Vector3.zero;
                cube.transform.parent = GameObject.Find("Notes").transform;
                
                noteCount++;
                timer -= beat;
            }

            timer += Time.deltaTime;
        }
        
    }
}
