using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool SmoothTransition = false;
    public float TransitionSpeed = 10f;
    public float TransitionRotationSpeed = 500f;

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    InputManager inputManager => InputManager.Instance;
    void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);

    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        if (true)
        {
            prevTargetGridPos = targetGridPos;

            Vector3 _targetPosition = targetGridPos;

            if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
            if (targetRotation.y < 0f) targetRotation.y = 270f;

            if (!SmoothTransition)
            {
                transform.position = _targetPosition;
                transform.rotation = Quaternion.Euler(targetRotation);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * TransitionSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * TransitionRotationSpeed);
            }
        }
        else
        {
            targetGridPos = prevTargetGridPos;
        }
    }
    void Update()
    {
        if (inputManager.W)
        {
            MoveForward();
        }
        if (inputManager.A)
        {
            RotateLeft();
        }
        if (inputManager.S)
        {
            MoveBackwards();
        }
        if (inputManager.D)
        {
            RotateRight();
        }
    }
    public void RotateLeft()
    {
        if (!atRest) return;
        targetRotation -= Vector3.up * 90f;
    }
    public void RotateRight()
    {
        if (!atRest) return;
        targetRotation += Vector3.up * 90f;
    }
    public void MoveForward()
    {
        if (!atRest) return;
        targetGridPos += transform.forward;
    }
    public void MoveBackwards()
    {




        // Space here done by Noki
        if (!atRest) return;
        targetGridPos -= transform.forward;
    }
    public void MoveLeft()
    {
        if (!atRest) return;
        targetGridPos -= Vector3.right;
    }
    public void MoveRight()
    {
        if (!atRest) return;
        targetGridPos += Vector3.right;
    }
    bool atRest
    {
        get
        {
            if ((Vector3.Distance(transform.position, targetGridPos) < 0.05f) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f))
                return true;
            else
                return false;
        }
    }

}
