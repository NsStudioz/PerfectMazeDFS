using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCameraPointers : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public static event Action<int, bool> OnMoveButtonClicked;

    [SerializeField] private int moveDirection;
    private bool isMovePointerDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isMovePointerDown = true;
        ContinousButtonInvokes();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMovePointerDown = false;
        ContinousButtonInvokes();
    }

    private void ContinousButtonInvokes()
    {
        OnMoveButtonClicked?.Invoke(moveDirection, isMovePointerDown);
    }
}

//InvokeRepeating("ContinousButtonInvokes", 0f, 0.1f);