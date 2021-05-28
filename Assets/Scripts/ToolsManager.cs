using RootMotion.FinalIK;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tool : MonoBehaviour
{
    public GameObject pointer;
    public GameObject[] sceneObjects;

    public void Pause() 
    {
        Debug.Log("Hiding tool");
        foreach (var o in sceneObjects)
        {
            Debug.Log("Hiding object" + o.name);
            o.SetActive(false);
        }
        pointer.SetActive(true);
    }

    public void Show()
    {
        Debug.Log("Showing tool");
        pointer.SetActive(false);
        foreach (var o in sceneObjects)
        {
            o.SetActive(true);
        }
    }
}

public class ToolsManager : MonoBehaviour
{
    public Tool[] tools;

    [Tooltip("The object to interact to")]
    public InteractionObject interactionObject;
    [Tooltip("The effectors to interact with")]
    public FullBodyBipedEffector[] effectors;
    [Tooltip("The body that has the interaction system")]
    public InteractionSystem interactionSystem;

    public GameObject fingerPlane;
    public GameObject indicator;

    public GameObject nose;
    public Collider left_hand;
    public Collider right_hand;
    public List<Collider> indexes_hand = new List<Collider>();

    public static ToolsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fingerPlane.GetComponent<MeshRenderer>().enabled = false;
        indicator.GetComponent<MeshRenderer>().enabled = false;
        nose.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            fingerPlane.transform.position = new Vector3(right_hand.transform.position.x, right_hand.transform.position.y, right_hand.transform.position.z * 0.9f);
            
            // Comments to show 90% extensibility
            fingerPlane.GetComponent<MeshRenderer>().enabled = true;
            
            Invoke("StartFingerInteraction", 1);
        }

    }

    // TODO: G�rer les exp�riences a faire en faisant un menu pour faire le bon tool

    void StartFingerInteraction()
    {
        if (interactionSystem != null)
        {
            foreach (FullBodyBipedEffector e in effectors)
            {
                interactionSystem.StartInteraction(e, interactionObject, true);
            }
        }
    }
}
