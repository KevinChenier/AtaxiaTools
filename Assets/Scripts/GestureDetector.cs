using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureDetector : MonoBehaviour
{
    public OVRSkeleton skeleton;
    private List<OVRBone> fingerBones;
    public List<Gesture> gestures;
    public bool debug = false;
    public float threshold = 0.1f;
    private Gesture previousGesture;
    private bool bonesFound = false;   

    // Start is called before the first frame update
    void Start()
    {
        previousGesture = new Gesture();
    }

    // Update is called once per frame
    void Update()
    {
        if (bonesFound)
        {
            if (debug && Input.GetKeyDown(KeyCode.Space))
            {
                Save();
            }

            Gesture currentGesture = Recognize();
            bool hasRecognized = gestures.Contains(currentGesture);

            if (hasRecognized && !currentGesture.Equals(previousGesture))
            {
                // new gesture was recognized
                Debug.Log("New gesture detected: " + currentGesture.name);
                previousGesture = currentGesture;
                currentGesture.onRecognized.Invoke();
            }
        }
        else
        {
            if(skeleton.Bones.Count > 0)
            {
                fingerBones = new List<OVRBone>(skeleton.Bones);
                bonesFound = true;
            }
        }
    }
     
    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        foreach(var bone in fingerBones)
        {
            // finger position relative to the root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                if(distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }
        return currentGesture;
    }
}
