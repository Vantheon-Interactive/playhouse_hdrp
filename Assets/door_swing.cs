using UnityEngine;

public class DoorSwing : MonoBehaviour
{
    public Vector3 closedRotation;      // Rotation when the door is closed
    public Vector3 openRotation;        // Rotation when the door is fully open
    public float swingSpeed = 1.0f;     // Speed of the door swinging

    public float minWaitTime = 1.0f;    // Minimum time to wait at open/close
    public float maxWaitTime = 3.0f;    // Maximum time to wait at open/close

    private bool isOpening = true;      // Whether the door is swinging open
    private bool isWaiting = false;     // Whether we're waiting at the end position
    private float waitTimer = 0f;       // Countdown for wait time
    private float swingProgress = 0f;   // Progress from 0 to 1

    void Start()
    {
        // Start from closed rotation
        transform.rotation = Quaternion.Euler(closedRotation);
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                isOpening = !isOpening;     // Toggle door direction
                swingProgress = 0f;         // Reset progress for next swing
            }
        }
        else
        {
            swingProgress += Time.deltaTime * swingSpeed;
            float t = Mathf.Clamp01(swingProgress);

            Quaternion from = Quaternion.Euler(isOpening ? closedRotation : openRotation);
            Quaternion to = Quaternion.Euler(isOpening ? openRotation : closedRotation);

            transform.rotation = Quaternion.Lerp(from, to, t);

            if (t >= 1f)
            {
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }
}
