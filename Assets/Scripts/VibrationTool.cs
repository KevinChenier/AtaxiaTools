using Assets.Scripts.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VibrationTool : Tool<VibrationConfig>
{
    private const float MINIMUM_STR = 0.0f; // Minimum for haptic activation is 0.0

    [Range(MINIMUM_STR, 1)]
    public float strength;

    private int repetition;
    private float timer;

    public VibrationTool() : base("vibration") { }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (toolBegan)
        {
            if (timer >= 0.25f && !toolEnded)
            {
                strength += 0.0005f;
                OVRInput.SetControllerVibration(1, strength, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(1, strength, OVRInput.Controller.RTouch);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    private void HandleVibrationCheck(object source, EventArgs args)
    {
        score();
        Debug.Log(strength);
        strength = MINIMUM_STR;

        repetition++;

        if (repetition >= configs.repetitions)
        {
            EndTool(5);
        }
    }

    public override void InitTool()
    {
        base.InitTool();

        ControllerInputEvent.Instance.TriggerEvent += HandleVibrationCheck;
    }

    public override void EndTool(int timer)
    {
        base.EndTool(timer);

        ControllerInputEvent.Instance.TriggerEvent -= HandleVibrationCheck;
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.VibrationConfig, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.VibrationConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            Repetitions = configs.repetitions
        });
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.VibrationData, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.VibrationData.ToString(),

            VibrationStrength = strength
        });
    }
}
