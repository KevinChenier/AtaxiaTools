using Assets.Scripts;
using System;
using System.Diagnostics;
using UnityEngine;
using Valve.VR;

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
        if(OVRInput.GetUp(OVRInput.Button.Start) || SteamVR_Input.GetStateDown("ActivateUI", SteamVR_Input_Sources.RightHand))
        {
            StartUpEvent(this, null);
        }
    }
}
