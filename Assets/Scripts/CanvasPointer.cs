using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasPointer : MonoBehaviour
{
    public float defaultLenght = 3.0f;

    public EventSystem eventSystem;
    public StandaloneInputModule inputModule;

    private LineRenderer lineRenderer;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 GetEnd()
    {
        float distance = GetCanvasDistance();
        var endPosition = CalculateEnd(distance == 0 ? defaultLenght : distance);

        if (distance != 0)
            endPosition = CalculateEnd(distance);

        return endPosition;
    }

    private Vector3 CalculateEnd(float distance)
    {
        return transform.position + (transform.forward * (float)distance);
    }

    private float GetCanvasDistance()
    {
        var eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;

        var results = new List<RaycastResult>();

        eventSystem.RaycastAll(eventData, results);

        var closest = FindFirstRaycast(results);
       return  Mathf.Clamp(closest?.distance ?? 0, 0, default);
    }

    private RaycastResult? FindFirstRaycast(IEnumerable<RaycastResult> results)
    {
        return results.FirstOrDefault(r => r.gameObject != null);
    }

    private Vector3 DefaultEnd()
    {
        return transform.position + transform.forward * defaultLenght;
    }
}
