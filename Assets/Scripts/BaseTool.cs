using Assets.Scripts;
using System.Diagnostics;
using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    public GameObject pointer;
    public GameObject[] sceneObjects;
    public IToolConfig baseConfigs { get; set; }
    public Tips tips;
    
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