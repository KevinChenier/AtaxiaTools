using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public Object note;

    public RhythmTool rhythmTool;

    private float timer;
    private int noteCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (noteCount < rhythmTool.configs.nbNotes) 
        {
            if (timer >= (60.0f / rhythmTool.configs.bpm))
            {
                GameObject cube = Instantiate(note, transform.position, Quaternion.identity) as GameObject;
                cube.transform.SetParent(this.transform);
                cube.transform.eulerAngles = Vector3.zero;
                
                noteCount++;
                timer = 0;
            }
            timer += Time.deltaTime;
        }   
    }
}