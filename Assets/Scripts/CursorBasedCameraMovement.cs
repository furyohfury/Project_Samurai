using UnityEngine;

public class CursorBasedCameraMovement : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float maxDistanceFromCenter = 0.1f;
    public float maxOffset = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, mainCamera.transform.position.y));
        Vector3 centerScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 centerWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(centerScreenPosition.x, centerScreenPosition.y, mainCamera.transform.position.y));

        float distanceFromCenter = Vector3.Distance(cursorWorldPosition, centerWorldPosition);

        if (distanceFromCenter > maxDistanceFromCenter)
        {
            Vector3 direction = cursorWorldPosition - centerWorldPosition;
            direction.y = 0; // We don't want to move the camera up or down

            float distanceRatio = Mathf.Clamp01((distanceFromCenter - maxDistanceFromCenter) / maxOffset);
            float moveDistance = moveSpeed * distanceRatio;

            Vector3 newPosition = transform.position + (direction.normalized * moveDistance);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10); // Smoothly move the camera
        }
    }
}
