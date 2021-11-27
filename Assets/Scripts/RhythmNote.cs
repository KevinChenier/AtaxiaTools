using UnityEngine;

public class RhythmNote : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-speed, 0, 0);
    }
}
