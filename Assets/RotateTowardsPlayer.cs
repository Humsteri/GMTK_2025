using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    void LateUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
