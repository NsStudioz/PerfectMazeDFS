using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PerfectMazeDFS_Pointers
{
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
}