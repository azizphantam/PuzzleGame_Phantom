using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 1.0f; // Adjust this to control sensitivity of camera movement
    public float minX = -20.0f; // Minimum X position
    public float maxX = 20.0f;  // Maximum X position

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x * sensitivity * Time.deltaTime;
                Vector3 newPos = transform.position + new Vector3(deltaX, 0, 0);
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX); // Clamp X position
                transform.position = newPos;
            }
        }
    }
}
