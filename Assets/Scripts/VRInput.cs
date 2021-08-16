using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInput : BaseInput
{
    public Camera eventCam = null;

    public OVRInput.Button clickButton = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller controller = OVRInput.Controller.All;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button)
    {
        return OVRInput.Get(clickButton, controller) || SteamVR_Input.GetState("InteractUI", SteamVR_Input_Sources.RightHand);
    }

    public override bool GetMouseButtonDown(int button)
    {
        return OVRInput.GetDown(clickButton, controller) || SteamVR_Input.GetStateDown("InteractUI", SteamVR_Input_Sources.RightHand);
    }

    public override bool GetMouseButtonUp(int button)
    {
        return OVRInput.GetUp(clickButton, controller) || SteamVR_Input.GetStateUp("InteractUI", SteamVR_Input_Sources.RightHand);
    }

    public override Vector2 mousePosition => eventCam.pixelRect.center;
}
