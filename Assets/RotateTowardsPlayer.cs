using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    Transform player => GameObject.FindGameObjectWithTag("Player").transform;
    void LateUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        transform.forward = Camera.main.transform.forward;
        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
