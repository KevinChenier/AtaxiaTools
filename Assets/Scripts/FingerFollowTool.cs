using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerFollowTool : Tool<FingerFollowConfig>
{
    public enum Mode
    {
        Normal,
        IncrementalSpeed,
        Target
    }
    
    Vector3 startPos;
    Vector3 endPos;

    public FingerFollowTool() : base("fingerFollow") { }

    [Range(0.1f, 1.0f)]
    public float speedModifier = 0.1f;

    public List<float> incrementalSpeeds;
    int currentSpeed = 0;

    public Mode FingerFollowMode = Mode.Normal;

    float lerpValue = 0.0f;
    private FindRandomPoint randomPoint;

    public GameObject Trajectory;
    List<GameObject> TrajectoryEndPoints = new List<GameObject>();
    int currentEndpoint = 1;

    void OnTriggerStay(Collider other)
    {
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            switch (FingerFollowMode)
            {
                case Mode.Normal:
                    HandleNormalFingerFollow();
                    break;
                case Mode.IncrementalSpeed:
                    HandleIncrementalFingerFollow();
                    break;
                case Mode.Target:
                    HandleTargetFingerFollow();
                    break;
            }
            AdvanceIndicator();
        }
    }

    void HandleNormalFingerFollow()
    {
        if (gameObject.transform.position == endPos)
        {
            startPos = gameObject.transform.position;
            endPos = randomPoint.CalculateRandomPoint();

            lerpValue = 0;
        }
    }

    void HandleTargetFingerFollow()
    {
        if (gameObject.transform.position == endPos)
        {
            startPos = gameObject.transform.position;
            currentEndpoint = currentEndpoint == TrajectoryEndPoints.Count - 1 ? 0 : currentEndpoint + 1;
            endPos = TrajectoryEndPoints[currentEndpoint].transform.position;

            lerpValue = 0;
        }
    }

    void HandleIncrementalFingerFollow()
    {
        if (gameObject.transform.position == endPos)
        {
            startPos = gameObject.transform.position;
            endPos = randomPoint.CalculateRandomPoint();

            lerpValue = 0;
            currentSpeed = currentSpeed == incrementalSpeeds.Count - 1 ? 0 : currentSpeed + 1;
            speedModifier = incrementalSpeeds[currentSpeed];
        }
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
        randomPoint = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();

        switch (FingerFollowMode)
        {
            case Mode.IncrementalSpeed:
                speedModifier = incrementalSpeeds[0];
                startPos = randomPoint.CalculateRandomPoint();
                endPos = randomPoint.CalculateRandomPoint();
                break;
            case Mode.Normal:
                startPos = randomPoint.CalculateRandomPoint();
                endPos = randomPoint.CalculateRandomPoint();
                break;
            case Mode.Target:
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

    public override int score()
    {
        throw new System.NotImplementedException();
    }
}
