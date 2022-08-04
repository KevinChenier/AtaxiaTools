using Assets.Scripts;
using System;
using System.Diagnostics;
using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    public GameObject pointer;
    public GameObject[] sceneObjects;
    public IToolConfig baseConfigs { get; set; }
    public bool toolBegan { get; set; }
    public bool toolEnded { get; set; }


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

    public virtual void InitTool()
    {
        if (toolBegan)
            throw new InvalidOperationException("Tool can only start once.");
    }

    public virtual void EndTool(int timer) 
    {
        if (toolEnded)
            throw new InvalidOperationException("Tool can only end once.");
    }
}