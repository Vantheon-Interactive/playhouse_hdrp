using UnityEngine;

public class rotate_portal : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 0, -90 * Time.deltaTime);
    }
}
