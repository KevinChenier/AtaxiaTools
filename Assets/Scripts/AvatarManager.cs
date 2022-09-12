using OculusSampleFramework;
using RootMotion.FinalIK;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    [Tooltip("The object to interact to")]
    public InteractionObject interactionObject;
    [Tooltip("The effectors to interact with")]
    public FullBodyBipedEffector[] effectors;

    public GameObject fingerPlane;
    public GameObject indicator;

    public GameObject left_hand;
    public GameObject right_hand;
    public List<Collider> indexes_hand = new List<Collider>();

    public static AvatarManager Instance { get; private set; }

    private bool _showAvatar = false;

    [Tooltip("The body that has the interaction system")]
    private InteractionSystem interactionSystem;

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
        interactionSystem = GetComponent<InteractionSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (fingerPlane != null)
            fingerPlane.GetComponent<MeshRenderer>().enabled = false;

        if (indicator != null)
            indicator.GetComponent<MeshRenderer>().enabled = false;
    }

    void CalibrateArm ()
    {
        if (fingerPlane != null)
        {
            fingerPlane.transform.position = new Vector3(right_hand.transform.position.x, right_hand.transform.position.y, right_hand.transform.position.z * 0.9f);

            fingerPlane.GetComponent<FindRandomPoint>().Recalculate();

            indicator.gameObject.transform.position = fingerPlane.GetComponent<FindRandomPoint>().CalculateRandomPoint();
        }
    }

    void StartFingerInteraction()
    {
        if (interactionSystem != null)
        {
            Debug.Log("Starting interactions");

            foreach (FullBodyBipedEffector e in effectors)
            {
                interactionSystem.StartInteraction(e, interactionObject, true);
            }
        }
    }

    private void OnEnable()
    {
        Invoke("StartFingerInteraction", 0.5f);
    }

    private void OnDestroy()
    {
        
    }

    public bool showAvatar
    {
        get { return _showAvatar; }
        set
        {
            gameObject.SetActive(value);
            indicator.GetComponent<MeshRenderer>().enabled = !value;
            _showAvatar = value;
        }
    }
}