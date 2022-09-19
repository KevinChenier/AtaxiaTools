using Assets.Scripts.Model;
using Obi;
using UnityEngine;

public class EverydayTaskTool : Tool<EverydayTaskConfig>
{
    public GameObject container;
    public GameObject recipient;
    public GameObject PourLine;
    public ObiEmitter emitter;
    public double accuracy { get; set; }

    private float timer;

    public EverydayTaskTool() : base("everydayTask") { }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EverydayTaskData, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EverydayTaskData.ToString(),

            Score = accuracy * 100.0 + "%",
            TimeTask = timer
        });
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EverydayTaskConfig, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EverydayTaskConfig.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,

            ToolEnded = toolEnded,
            PourHeight = configs.height
        });
    }

    public override void InitTool()
    {
        base.InitTool();
        PourLine.transform.position = new Vector3(PourLine.transform.position.x, configs.height, PourLine.transform.position.z);
        GeneralDataExtractor.Instance.StartSaveOculusControllersData(Assets.Scripts.Model.Types.EventType.EverydayTaskData.ToString());
    }

    public override void EndTool(int timer)
    {
        score();
        GeneralDataExtractor.Instance.CancelSaveOculusControllersData();
        base.EndTool(timer);
    }

    private void CheckFluidInContainer()
    {
        if (!container.GetComponent<ContainerCollider>().initialized || toolEnded)
            return;

        for (int i = 0; i < emitter.particleCount; i++)
        {
            Vector3 particlePosition = emitter.GetParticlePosition(i);

            if (container.GetComponent<CapsuleCollider>().bounds.Contains(particlePosition))
            {
                CancelInvoke("HandleEndTool");
                return;
            }
        }
        if (!IsInvoking("HandleEndTool"))
            Invoke("HandleEndTool", 5.0f);
    }

    private void CalculateAccuracy()
    {
        if (!container.GetComponent<ContainerCollider>().initialized || toolEnded)
            return;

        int accuracy = 0;

        for (int i = 0; i < emitter.particleCount; i++)
        {
            Vector3 particlePosition = emitter.GetParticlePosition(i);

            if (recipient.GetComponent<CapsuleCollider>().bounds.Contains(particlePosition))
                accuracy++;
        }
        this.accuracy = (double)accuracy / emitter.particleCount;
    }

    private void FixedUpdate()
    {
        CalculateAccuracy();
        CheckFluidInContainer();

        if (container.GetComponent<ContainerCollider>().initialized && !toolEnded)
        {
            timer += Time.deltaTime;
        }
    }

    void HandleEndTool()
    {
        Debug.Log(this.accuracy);
        EndTool(5);
    }
}
