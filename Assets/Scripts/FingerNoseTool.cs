using Assets.Scripts.Model;
using UnityEngine;

public class FingerNoseTool : Tool<FingerNoseConfig>
{
    public FingerNoseTool() : base("fingerNose") { }
    public GameObject nose;
    private int repetitions;
    private long lastTime;

    public void Recalculate()
    {
        gameObject.transform.position = AvatarManager.Instance.fingerPlane.GetComponent<FindRandomPoint>().CalculateRandomPoint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (AvatarManager.Instance.indexes_hand.Contains(other))
        {
            repetitions++;

            if (repetitions >= base.configs.repetitions)
            {
                EndTool(5);
            }
            else
            {
                score();

                nose.GetComponent<MeshRenderer>().enabled = true;
                nose.GetComponent<SphereCollider>().enabled = true;
                Recalculate();
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    public override void InitTool()
    {
        base.InitTool();

        nose.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.FingerNoseData, new
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.FingerNoseData.ToString(),

            Delay = time - lastTime,
            CurrentRepetition = repetitions
        });

        lastTime = time;
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.FingerNoseConfig, new
        {
            Time = System.DateTime.Now,
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.FingerNoseConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            Repetitions = configs.repetitions
        });
    }
}