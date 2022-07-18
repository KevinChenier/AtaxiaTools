using Assets.Scripts.Model;
using Obi;
using UnityEngine;

public class EverydayTaskTool : Tool<EverydayTaskConfig>
{
    public GameObject container;
    public GameObject recipient;
    public GameObject PourLine;
    public ObiSolver solver;
    public double accuracy { get; set; }

    private float timer;

    public EverydayTaskTool() : base("everydayTask") { }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.EverydayTaskData, new
        {
            Time = System.DateTime.Now,
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
            Time = System.DateTime.Now,
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
    }

    public override void EndTool(int timer)
    {
        score();
        base.EndTool(timer);
    }

    protected override void Update()
    {
        base.Update();
        if (container.GetComponent<ContainerCollider>().initialized && !toolEnded)
        {
            timer += Time.deltaTime;
        }
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        int accuracy = 0;

        // just iterate over all contacts in the current frame:
        foreach (Oni.Contact contact in e.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance < 0.1)
            {
                ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;

                if (col != null && col.name == recipient.name)
                    accuracy++;

                if (col != null && col.name == container.name)
                    // Still fluid in Container
                    return;
            }
        }
        if (container.GetComponent<ContainerCollider>().initialized && !toolEnded)
        {
            this.accuracy = (double) accuracy / e.contacts.Count;
            EndTool(5);
        }
    }
}
