using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        transform.LookAt(player);
    }
}
