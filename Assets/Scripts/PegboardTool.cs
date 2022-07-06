using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegboardTool : Tool<PegboardConfig>
{
    public List<Collider> toolObjects;
    public TextMesh toolText;
    public float timeGrab { get; set; }

    private int numberOfCurrentObjects = 0;
    private int numberOfObjects = 0;

    public PegboardTool() : base("pegboard") { }

    private void OnTriggerEnter(Collider other)
    {
        if (toolObjects.Contains(other))
        {
            numberOfCurrentObjects++;

            if (numberOfCurrentObjects >= numberOfObjects)
            {
                toolText.GetComponent<MeshRenderer>().enabled = true;
                toolText.text = "Test Succeeded!";
            }
            else
                toolText.text = numberOfCurrentObjects.ToString();
        }
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.PegboardData, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.PegboardData.ToString(),

            TimeTask = timeGrab
        });
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.PegboardConfig, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.PegboardConfig.ToString(),

            ToolEnded = toolEnded,
            TimerShowed = configs.isTimerShowed
        });
    }

    public override void InitTool()
    {
        base.InitTool();

        toolText.text = "0";
        toolText.GetComponent<MeshRenderer>().enabled = base.configs.isTimerShowed;
        numberOfObjects = toolObjects.Count;
    }

    public override void EndTool(int timer)
    {
        score();
        base.EndTool(timer);
    }
}
