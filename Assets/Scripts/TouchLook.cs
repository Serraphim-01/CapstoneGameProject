using UnityEngine;

public class TouchLook : MonoBehaviour
{
    public float lookSpeed = 0.2f;
    public Transform targetToRotate;

    private Vector2 lastTouchPos;
    private bool isLooking = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > Screen.width / 2) // right side
            {
                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPos = touch.position;
                    isLooking = true;
                }
                else if (touch.phase == TouchPhase.Moved && isLooking)
                {
                    Vector2 delta = touch.deltaPosition;
                    targetToRotate.Rotate(Vector3.up, delta.x * lookSpeed, Space.World);
                    lastTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isLooking = false;
                }
            }
        }
    }
}
