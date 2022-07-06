using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;
using System.Linq;

public class EyeTrackingMultipleTool : Tool<EyeTrackingMultipleConfig>
{
    public List<GameObject> eyeTrackingMultipleTargetsPattern;
    public GameObject Targets;
    public double timer { get; set; }
    public int currentObjectIndex { get; set; }

    public EyeTrackingMultipleTool() : base("eyeTrackingMultiple") { }

    public override void InitTool()
    {
        base.InitTool();

        calibrateTargets();

        foreach (GameObject eyeTrackingMultipleObject in eyeTrackingMultipleTargetsPattern)
        {
            eyeTrackingMultipleObject.transform.localScale *= (float)base.configs.targetSize;
        }

        GameObject currentObject = eyeTrackingMultipleTargetsPattern[currentObjectIndex];
        currentObject.GetComponent<EyeTrackingMultipleGazeAt>()._targetColor = currentObject.GetComponent<EyeTrackingMultipleGazeAt>().highlightColor;
        currentObject.GetComponent<EyeTrackingMultipleGazeAt>().currentObjectToGazeAt = true;

        timer = base.configs.timer;
    }

    private void calibrateTargets()
    {
        var list = Targets.GetComponentsInChildren<Transform>();
        Targets.transform.position = new Vector3(Targets.transform.position.x, Camera.main.transform.position.y, Targets.transform.position.z);

        for (int i = 1; i < list.Length; i++)
        {
            Vector3 direction = list[i].transform.position - list[0].transform.position;
            direction = direction.normalized;
            float distance = Vector3.Distance(Camera.main.transform.position, list[0].transform.position) * Mathf.Tan(Mathf.Deg2Rad * base.configs.fieldOfView / 2);
            list[i].transform.position = list[0].transform.position;
            list[i].transform.Translate(direction * distance);
        }

        // Temporary fix for bug concerning transforms of canvas when moving parent (Targets)
        foreach (Canvas canvas in Targets.GetComponentsInChildren<Canvas>())
        {
            canvas.transform.localPosition = new Vector3(0, 0, -0.5470001f);
        }

    }

    public override void score()
    {
        throw new System.NotImplementedException();
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeTrackingMultipleConfig, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeTrackingMultipleConfig.ToString(),

            ToolEnded = toolEnded,
            TargetSize = configs.targetSize,
            TargetTimer = configs.timer
        });
    }
}
