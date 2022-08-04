using Assets.Scripts;
using Autohand.Demo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Valve.VR;

public class ControllerInputEvent : MonoBehaviour
{
    private static ControllerInputEvent _instance;
    public event EventHandler StartUpEvent;
    public event EventHandler TriggerEvent;
    public event EventHandler SkipEvent;
    public event EventHandler RestartEvent;
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
        if (!ConfigManager.Instance.Config.ScenarioActive)
        {
            if (OVRInput.GetUp(OVRInput.Button.Start)
            || SteamVR_Input.GetStateDown("ActivateUI", SteamVR_Input_Sources.RightHand))
            {
                if(StartUpEvent != null)
                    StartUpEvent(this, null);
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            if(TriggerEvent != null)
                TriggerEvent(this, null);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(OVRManager.instance)
                OVRManager.display.RecenterPose();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (RestartEvent != null)
                RestartEvent(this, null);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if(SkipEvent != null)
                SkipEvent(this, null);
        }
    }
}
