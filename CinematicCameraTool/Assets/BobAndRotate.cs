using UnityEngine;

public class BobAndRotate : MonoBehaviour
{
    public float bobAmplitude = 0.5f;
    public float bobFrequency = 1f;
    public float rotationSpeed = 30f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Bob up and down
        float newY = startPosition.y + bobAmplitude * Mathf.Sin(Time.time * bobFrequency * 2f * Mathf.PI);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
