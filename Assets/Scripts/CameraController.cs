using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float followSpeed = 5.0f;
    public Vector3 offset = new(0, 0, -10);

    public bool useBoundaries;
    public Vector2 minBoundary;
    public Vector2 maxBoundary;
    

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);

        if (useBoundaries)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBoundary.x, maxBoundary.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBoundary.y, maxBoundary.y);
        }

        transform.position = smoothedPosition;
    }
}
