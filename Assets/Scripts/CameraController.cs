using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 newCameraPos = Vector3.zero;

    private int minValue = -250;
    private int maxValue = 250;


    private void Start()
    {
        ClampCameraPositions();
        transform.position = newCameraPos;
    }

    private void ClampCameraPositions()
    {
        newCameraPos.x = Mathf.Clamp(minValue, 0, maxValue);
        newCameraPos.z = Mathf.Clamp(minValue, 0, maxValue);
    }

}
