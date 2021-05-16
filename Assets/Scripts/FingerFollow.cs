using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerFollow : Tool
{
    Vector3 startPos;
    Vector3 endPos;

    [Range(0.1f, 5.0f)]
    public float speedModifier = 0.5f;
    float lerpValue = 0.0f;

    private FindRandomPoint randomPoint;

    void Start()
    {
        randomPoint = ToolsManager.Instance.fingerPlane.GetComponent<FindRandomPoint>();
    }

    void OnTriggerStay(Collider other)
    {
        if (ToolsManager.Instance.indexes_hand.Contains(other))
        {
            if (startPos.Equals(Vector3.zero) || gameObject.transform.position == endPos)
            {
                startPos = gameObject.transform.position;
                endPos = randomPoint.CalculateRandomPoint();

                lerpValue = 0;
            }
            // TODO: Correct speed for constant speed 
            lerpValue += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, lerpValue * speedModifier);
        }
    }
}
