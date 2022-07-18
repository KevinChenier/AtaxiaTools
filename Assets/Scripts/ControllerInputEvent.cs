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
    private List<InputDevice> devices = new List<InputDevice>();
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
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
            if (OVRInput.GetUp(OVRInput.Button.Start)
            || SteamVR_Input.GetStateDown("ActivateUI", SteamVR_Input_Sources.RightHand))
            {
                StartUpEvent(this, null);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OVRManager.display.RecenterPose();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
