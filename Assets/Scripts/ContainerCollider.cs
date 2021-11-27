using UnityEngine;
using Autohand;

public class ContainerCollider : MonoBehaviour
{
    public EverydayTaskTool everydayTaskTool;
    
    public GameObject PourLine;
    public GameObject Cap;
    public bool initialized { get; set; }

    void Start()
    {
        // Can't grab container when fluid is pouring
        deactivateContainer();

        // Let the time pass for fluid
        Invoke("activateContainer", 4);
    }

    void openCap()
    {
        Cap.SetActive(false);
    }

    void closeCap()
    {
        Cap.SetActive(true);
    }

    void activateContainer()
    {
        GetComponent<OVRGrabbable>().enabled = true;
        GetComponent<Grabbable>().enabled = true;
        closeCap();
        initialized = true;

        Debug.Log("Container initialized!");
    }

    void deactivateContainer()
    {
        GetComponent<OVRGrabbable>().enabled = false;
        GetComponent<Grabbable>().enabled = false;
        openCap();
    }

    private void Update()
    {
        if (initialized)
        {
            if (transform.position.y >= PourLine.transform.position.y)
            {
                openCap();
            }
            else
            {
                closeCap();
            }
        }
    }
}
