using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PerfectMazeDFS.MovePointers
{
    public class MoveCameraPointers : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// This script is attached to UI buttons, specifically movement buttons for the camera.
        /// Once clicked on a specific button, an event will be triggered and the listener will move the camera.
        /// The camera will move according to the direction of arrow signs on each button.
        /// The movement is made by passing a number 'moveDirection' to a movement function in the CameraController script.
        /// </summary>

        public static event Action<int, bool> OnMoveButtonClicked;

        [SerializeField] private int moveDirection;
        private bool isMovePointerDown = false;

        /// <summary>
        /// trigger event on mouse click and hold, this will pass the index number of the button to the MoveCameraWithPointers function
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            isMovePointerDown = true;
            ContinousButtonInvokes();
        }

        /// <summary>
        /// trigger event on mouse click released, this will pass the index number of the button to the MoveCameraWithPointers function
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            isMovePointerDown = false;
            ContinousButtonInvokes();
        }

        // invoke the event, pass the proper parameters to the listener:
        private void ContinousButtonInvokes()
        {
            OnMoveButtonClicked?.Invoke(moveDirection, isMovePointerDown);
        }
    }
}