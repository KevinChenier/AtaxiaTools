using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroll : MonoBehaviour
{
    public GameObject DestroyPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * - transform.right * 0.5f;

        if(transform.position.x <= DestroyPoint.transform.position.x)
        {
            Destroy(this.gameObject);
        }
    }

}
