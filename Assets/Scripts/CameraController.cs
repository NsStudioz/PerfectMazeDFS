using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 newCameraPos = Vector3.zero;
    [SerializeField] private float moveSpeed; 

    private int minValue = -250;
    private int maxValue = 250;


    private void Start()
    {
        ClampCameraPositions();
        UpdateNewPosition();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            MoveCameraUp();

        else if (Input.GetKey(KeyCode.S))
            MoveCameraDown();

        if (Input.GetKey(KeyCode.D))
            MoveCameraRight();

        else if (Input.GetKey(KeyCode.A))
            MoveCameraLeft();
    }

    private void ClampCameraPositions()
    {
        newCameraPos.x = Mathf.Clamp(minValue, 0, maxValue);
        newCameraPos.z = Mathf.Clamp(minValue, 0, maxValue);
    }

    private void MoveCameraUp()
    {
        newCameraPos.z += MovementSpeed();
        UpdateNewPosition();
    }

    private void MoveCameraDown()
    {
        newCameraPos.z -= MovementSpeed();
        UpdateNewPosition();
    }

    private void MoveCameraRight()
    {
        newCameraPos.x += MovementSpeed();
        UpdateNewPosition();
    }

    private void MoveCameraLeft()
    {
        newCameraPos.x -= MovementSpeed();
        UpdateNewPosition();
    }

    private float MovementSpeed()
    {
        return moveSpeed * Time.deltaTime;
    }

    private void UpdateNewPosition()
    {
        transform.position = newCameraPos;
    }

}
