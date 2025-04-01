using UnityEngine;

public class DummyPlayerMover : MonoBehaviour
{
    public float radius = 2f;
    public float speed = 1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.Cos(Time.time * speed) * radius;
        float z = Mathf.Sin(Time.time * speed) * radius;
        transform.position = startPos + new Vector3(x, 0, z);
    }
}