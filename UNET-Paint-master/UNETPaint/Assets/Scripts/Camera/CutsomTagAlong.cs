using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsomTagAlong : MonoBehaviour {

    [Tooltip("Sphere radius.")]
    public float Radius = 1.0f;

    [Tooltip("How fast the object will move to the target position.")]
    public float MoveSpeed = 2.0f;

    private Vector3 targetPosition;
    private Vector3 optimalPosition;
    private float initialDistanceToCamera;

    void Start()
    {
        initialDistanceToCamera = Vector3.Distance(this.transform.position, Camera.main.transform.position);
    }

    void Update()
    {
        optimalPosition = Camera.main.transform.position + Camera.main.transform.forward * initialDistanceToCamera;
        transform.rotation = Camera.main.transform.rotation;

        Vector3 offsetDir = this.transform.position - optimalPosition;
        if (offsetDir.magnitude > Radius)
        {
            targetPosition = optimalPosition + offsetDir.normalized * Radius;

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, MoveSpeed);
        }
    }
}
