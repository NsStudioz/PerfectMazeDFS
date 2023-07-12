using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 newCameraPos = Vector3.zero;
    [SerializeField] private float moveSpeed; 

    private int XZminValue = -250;
    private int XZmaxValue = 250;
    private int YminValue = -95;
    private int YmaxValue = 150;


    private void Start()
    {
        ClampCameraPositions();
        UpdateNewPosition();
    }

    private void Update()
    {
        ClampCameraPositions();

        if (Input.GetKey(KeyCode.W))
            MoveCameraUp();

        else if (Input.GetKey(KeyCode.S))
            MoveCameraDown();

        if (Input.GetKey(KeyCode.D))
            MoveCameraRight();

        else if (Input.GetKey(KeyCode.A))
            MoveCameraLeft();

        if (Input.GetKey(KeyCode.E))
            ZoomIn();

        else if (Input.GetKey(KeyCode.Q))
            ZoomOut();
    }

    private void ZoomIn()
    {
        newCameraPos.y -= MovementSpeed();
        UpdateNewPosition();
    }

    private void ZoomOut()
    {
        newCameraPos.y += MovementSpeed();
        UpdateNewPosition();
    }

    private void ClampCameraPositions()
    {
        newCameraPos.x = Mathf.Clamp(newCameraPos.x, XZminValue, XZmaxValue);
        newCameraPos.z = Mathf.Clamp(newCameraPos.z, XZminValue, XZmaxValue);
        newCameraPos.y = Mathf.Clamp(newCameraPos.y, YminValue, YmaxValue);
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
