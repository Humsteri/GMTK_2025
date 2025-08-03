using UnityEngine;
using PrimeTween;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] float addedFovAmount;
    [SerializeField] float fovChangeSpeed;
    [SerializeField] Ease cameraEase;
    [SerializeField] float cameraPosChangeAmountY;
    [SerializeField] float cameraPosChangeSpeed;
    [SerializeField] bool cameraAnimation;
    [SerializeField] bool fovChange;
    float cameraPosY;
    float cameraFov;

    [Header("Movement transition")]
    public bool SmoothTransition = false;
    public float TransitionSpeed = 10f;
    public float TransitionRotationSpeed = 500f;

    [Header("Movement amount")]
    public float MovementFactor = 1f;

    [Header("Raycast settings")]
    public float CollideRayLength = 2f;
    [SerializeField] Transform raycastStartPoint;
    [SerializeField] LayerMask collideLayer;
    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;
    bool teleporting;
    InputManager inputManager => InputManager.Instance;
    void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
        cameraPosY = mainCamera.transform.position.y;
        cameraFov = mainCamera.fieldOfView;
        
    }
    public void TeleportPlayer(Vector3 pos)
    {
        teleporting = true;
        targetGridPos = Vector3Int.RoundToInt(pos);
        
        teleporting = false;
    }  
    void FixedUpdate()
    {
        if(!teleporting)
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
    bool CollideForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, CollideRayLength, collideLayer))
        {
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            return false;
        }
    }
    bool CollideBack()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastStartPoint.position, transform.TransformDirection(Vector3.back), out hit, CollideRayLength, collideLayer))
        {
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 1000, Color.white);
            return false;
        }
    }
    public void CameraWalkAnimation()
    {
        Tween.PositionY(mainCamera.transform, mainCamera.transform.position.y - cameraPosChangeAmountY, cameraPosY, cameraPosChangeSpeed);
    }
    void Update()
    {
        if (inputManager.W)
        {
            if (!CollideForward())
            {
                MoveForward();
                AudioManager.Instance.PlayFootStep(transform.position);
                if (cameraAnimation)
                    CameraWalkAnimation();
                mainCamera.fieldOfView = cameraFov;
                if(fovChange)
                    Tween.CameraFieldOfView(mainCamera, mainCamera.fieldOfView + addedFovAmount, cameraFov, fovChangeSpeed, cameraEase);
            }
        }
        if (inputManager.A)
        {
            RotateLeft();
            AudioManager.Instance.PlayTurn(transform.position);
        }
        if (inputManager.S)
        {
            if (!CollideBack())
            {
                MoveBackwards();
                AudioManager.Instance.PlayFootStep(transform.position);
            }
        }
        if (inputManager.D)
        {
            RotateRight();
            AudioManager.Instance.PlayTurn(transform.position);
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
        targetGridPos += transform.forward * MovementFactor;
    }
    public void MoveBackwards()
    {




        // Space here done by Noki
        if (!atRest) return;
        targetGridPos -= transform.forward * MovementFactor;
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
