using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallpringJoint;

    private bool isDragging;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    private void Update()
    {
        if(currentBallRigidBody == null) { return; }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
           
            return;
        }

        isDragging = true;
        currentBallRigidBody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = worldPosition;

        
    }

    private void SpawnNewBall()
    {
      GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallpringJoint.connectedBody = pivot;
    }
    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallpringJoint.enabled = false;
        currentBallpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

}
