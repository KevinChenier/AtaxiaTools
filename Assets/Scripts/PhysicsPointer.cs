using UnityEngine;

public class PhysicsPointer : MonoBehaviour
{
    public float defaultLenght = 3.0f;

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
        lineRenderer.SetPosition(1, CalculateEnd());
    }

    private Vector3 CalculateEnd()
    {
        var hit = CreateFowardRaycast();
        var end = DefaultEnd();

        if (hit.collider)
            end = hit.point;
        return end;
    }

    private RaycastHit CreateFowardRaycast()
    {
        var ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out var hit, defaultLenght);

        return hit;
    }

    private Vector3 DefaultEnd()
    {
        return transform.position + transform.forward * defaultLenght;
    }
}
