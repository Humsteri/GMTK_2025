using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    Transform player => GameObject.FindGameObjectWithTag("Player").transform;
    public bool Smooth;
    void LateUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        if (!Smooth)
            transform.forward = Camera.main.transform.forward;
        else
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
