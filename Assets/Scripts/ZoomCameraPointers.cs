using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomCameraPointers : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<int, bool> OnZoomButtonClicked;

    [SerializeField] private int zoomDirection;
    private bool isZoomPointerDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isZoomPointerDown = true;
        ContinousButtonInvokes();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isZoomPointerDown = false;
        ContinousButtonInvokes();
    }

    private void ContinousButtonInvokes()
    {
        OnZoomButtonClicked?.Invoke(zoomDirection, isZoomPointerDown);
    }
}
