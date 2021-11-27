using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FingerFollowTool : Tool<FingerFollowConfig>
{
    Vector3 startPos;
    Vector3 endPos;

    public FingerFollowTool() : base("fingerFollow") { }

    [Range(0.1f, 1.0f)]
    public float speedModifier = 0.1f;

    public List<float> incrementalSpeeds;
    private int currentSpeed = 0;

    public Assets.Scripts.Model.Types.Mode FingerFollowMode;

    private float lerpValue = 0.0f;
    private FindRandomPoint randomPoint;

    public GameObject Trajectory;
    private List<GameObject> TrajectoryEndPoints = new List<GameObject>();
    private int currentEndpoint = 1;

    private bool lostFocus = false;
    private float lostFocusTimeInstance;

    private int repetitions = 0;

    void OnTriggerStay(Collider other)
    {
        if (AvatarManager.Instance.indexes_hand.Contains(other))
        {
            if (gameObject.transform.position == endPos)
            {
                repetitions++;

                if (repetitions >= base.configs.repetitions)
                {
                    EndTool(5);
                }
                else
                {
                    switch (FingerFollowMode)
                    {
                        case Assets.Scripts.Model.Types.Mode.Normal:
                            HandleNormalFingerFollow();
                            break;
                        case Assets.Scripts.Model.Types.Mode.IncrementalSpeed:
                            HandleIncrementalFingerFollow();
                            break;
                        case Assets.Scripts.Model.Types.Mode.Target:
                            HandleTargetFingerFollow();
                            break;
                    }
                }
            }
            else
            {
                AdvanceIndicator();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (AvatarManager.Instance.indexes_hand.Contains(other))
        {
            score();
            lostFocus = false;
            lostFocusTimeInstance = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (AvatarManager.Instance.indexes_hand.Contains(other))
        {
            lostFocus = true;
        }
    }

    void Update()
    {
        if (lostFocus)
        {
            lostFocusTimeInstance += Time.deltaTime;
        }
    }

    void HandleNormalFingerFollow()
    {
        lerpValue = 0;
        startPos = gameObject.transform.position;
        endPos = randomPoint.CalculateRandomPoint();
    }

    void HandleTargetFingerFollow()
    {
        lerpValue = 0;
        startPos = gameObject.transform.position;
        currentEndpoint = currentEndpoint == TrajectoryEndPoints.Count - 1 ? 0 : currentEndpoint + 1;
        endPos = TrajectoryEndPoints[currentEndpoint].transform.position;
    }

    void HandleIncrementalFingerFollow()
    {
        lerpValue = 0;
        startPos = gameObject.transform.position;
        endPos = randomPoint.CalculateRandomPoint();

        currentSpeed = currentSpeed == incrementalSpeeds.Count - 1 ? 0 : currentSpeed + 1;
        speedModifier = incrementalSpeeds[currentSpeed];
    }

    void SetTargetEndPoints()
    {
        for(int i = 0; i < Trajectory.transform.childCount; i++)
        {
            if (Trajectory.transform.GetChild(i).name.Contains("EndPoint"))
                TrajectoryEndPoints.Add(Trajectory.transform.GetChild(i).gameObject);
        }
    }

    void AdvanceIndicator()
    {
        lerpValue += Time.deltaTime;
        gameObject.transform.position = Vector3.Lerp(startPos, endPos, (lerpValue * speedModifier) / Vector3.Distance(startPos, endPos));
    }

    protected override void InitTool()
    {
        base.InitTool();

        randomPoint = AvatarManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();

        FingerFollowMode = base.configs.mode;

        switch (FingerFollowMode)
        {
            case Assets.Scripts.Model.Types.Mode.IncrementalSpeed:
                speedModifier = incrementalSpeeds[0];
                startPos = randomPoint.CalculateRandomPoint();
                endPos = randomPoint.CalculateRandomPoint();
                break;
            case Assets.Scripts.Model.Types.Mode.Normal:
                startPos = randomPoint.CalculateRandomPoint();
                endPos = randomPoint.CalculateRandomPoint();
                break;
            case Assets.Scripts.Model.Types.Mode.Target:
                if (Trajectory)
                {
                    Trajectory.SetActive(true);
                    SetTargetEndPoints();
                    startPos = TrajectoryEndPoints[0].transform.position;
                    endPos = TrajectoryEndPoints[1].transform.position;
                }
                break;
        }
        // Initialize indicator position depending on tool mode
        gameObject.transform.position = startPos;
    }

    public override void score()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.FingerNoseData, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.FingerNoseData.ToString(),
            
            LostFocusTime = lostFocusTimeInstance,
            CurrentRepetition = repetitions
        });
    }

    public override void configsSave()
    {
        var time = sw.ElapsedMilliseconds;

        bus.Push(Assets.Scripts.Model.Types.EventType.FingerFollowConfig, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.FingerFollowConfig.ToString(),

            ToolEnded = toolEnded,
            Repetitions = configs.repetitions
        });
    }
}
