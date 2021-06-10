using System;
using UnityEngine;

public class ControllerInputEvent : MonoBehaviour
{
    private static ControllerInputEvent _instance;

    public event EventHandler StartUpEvent;
    public static ControllerInputEvent Instance => _instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetUp(OVRInput.Button.Start))
        {
            StartUpEvent(this, null);
        }
    }
}
