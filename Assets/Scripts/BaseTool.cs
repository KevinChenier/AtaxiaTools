using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    public GameObject pointer;
    public GameObject[] sceneObjects;

    public void Pause()
    {
        foreach (var o in sceneObjects)
        {
            o.SetActive(false);
        }
        pointer.SetActive(true);
    }

    public void Show()
    {
        pointer.SetActive(false);
        foreach (var o in sceneObjects)
        {
            o.SetActive(true);
        }
    }
}