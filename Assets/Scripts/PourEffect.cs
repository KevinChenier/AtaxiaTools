using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourEffect : MonoBehaviour
{
    public ParticleSystem liquidSystem;

    // Start is called before the first frame update
    void Start()
    {
        liquidSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Angle(Vector3.down, transform.forward) <= 90f)
        {
            liquidSystem.Play();
        } 
        else
        {
            liquidSystem.Stop();
        }
        
    }
}
