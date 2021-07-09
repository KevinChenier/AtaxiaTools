using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Model;

public class EyeTrackingMultipleTool : Tool<EyeTrackingMultipleConfig>
{
    public List<GameObject> eyeTrackingMultipleObjects;
    public double timer = 3.0;

    public EyeTrackingMultipleTool() : base("eyeTrackingMultiple") { }

    protected override void InitTool()
    {
        int randomObjectIndex = Random.Range(0, eyeTrackingMultipleObjects.Count - 1);
        GameObject randomObject = eyeTrackingMultipleObjects[randomObjectIndex];
        randomObject.GetComponent<EyeTrackingMultipleGazeAt>()._targetColor = randomObject.GetComponent<EyeTrackingMultipleGazeAt>().highlightColor;
        randomObject.GetComponent<EyeTrackingMultipleGazeAt>().currentObjectToGazeAt = true;

        timer = base.configs.timer;
    }

    public override int score()
    {
        throw new System.NotImplementedException();
    }
}
