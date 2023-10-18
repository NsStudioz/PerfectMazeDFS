using PerfectMazeDFS.MovePointers;
using PerfectMazeDFS.ZoomPointers;
using UnityEngine;

namespace PerfectMazeDFS
{
    public class CameraController : MonoBehaviour
    {

        [Header("Elements")]
        [SerializeField] private Vector3 newCameraPos;
        [SerializeField] private float moveSpeed;

        // Move:
        private int moveIndex;
        private bool isMovePointerDown;
        // Zoom:
        private int zoomIndex;
        private bool isZoomPointerDown;

        // camera Clamping limits:
        private int XZminValue = 0;
        private int XZmaxValue = 275;
        private int YminValue = -90;
        private int YmaxValue = 100;

        #region Helpers:

        private float MovementSpeed()
        {
            return moveSpeed * Time.deltaTime;
        }

        // Update position of the camera:
        private void UpdateNewPosition()
        {
            transform.position = newCameraPos;
        }

        // Camera boundries, this will prevent camera from moving too far away from screen:
        private void ClampCameraPositions()
        {
            newCameraPos.x = Mathf.Clamp(newCameraPos.x, XZminValue, XZmaxValue);
            newCameraPos.z = Mathf.Clamp(newCameraPos.z, XZminValue, XZmaxValue);
            newCameraPos.y = Mathf.Clamp(newCameraPos.y, YminValue, YmaxValue);
        }

        #endregion

        private void Start()
        {
            ClampCameraPositions();
            UpdateNewPosition();
        }

        private void OnEnable()
        {
            MoveCameraPointers.OnMoveButtonClicked += SetMovePointersState;
            ZoomCameraPointers.OnZoomButtonClicked += SetZoomPointersState;
        }

        private void OnDisable()
        {
            MoveCameraPointers.OnMoveButtonClicked -= SetMovePointersState;
            ZoomCameraPointers.OnZoomButtonClicked -= SetZoomPointersState;
        }

        private void Update()
        {
            MoveCameraWithPointers();
            ZoomCameraWithPointers();
            ClampCameraPositions();
        }

        #region Pointer_Listeners:

        /// <summary>
        /// Event listener for MoveCameraPointers. This will trigger camera movement:
        /// </summary>
        /// <param name="moveIndex"></param>
        /// <param name="isMovePointerDown"></param>
        private void SetMovePointersState(int moveIndex, bool isMovePointerDown)
        {
            this.moveIndex = moveIndex;
            this.isMovePointerDown = isMovePointerDown;
        }

        /// <summary>
        /// Event listener for MoveCameraPointers. This will trigger camera zoom:
        /// </summary>
        /// <param name="zoomIndex"></param>
        /// <param name="isZoomPointerDown"></param>
        private void SetZoomPointersState(int zoomIndex, bool isZoomPointerDown)
        {
            this.zoomIndex = zoomIndex;
            this.isZoomPointerDown = isZoomPointerDown;
        }

        private void MoveCameraWithPointers() // 0 - Right, 1 - Left, 2 - Up, 3 - Down
        {
            // if mouse pointer released:
            if (!isMovePointerDown)
                return;

            // if mouse click and held:
            if (moveIndex == 0)
                MoveCameraRight();
            else if (moveIndex == 1)
                MoveCameraLeft();
            else if (moveIndex == 2)
                MoveCameraUp();
            else if (moveIndex == 3)
                MoveCameraDown();
        }

        private void ZoomCameraWithPointers() // 0 - Right, 1 - Left, 2 - Up, 3 - Down
        {
            // if mouse pointer released:
            if (!isZoomPointerDown)
                return;

            // if mouse click and held:
            if (zoomIndex == 0)
                ZoomIn();
            else if (zoomIndex == 1)
                ZoomOut();
        }

        #endregion

        #region Camera Movement:

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

        #endregion

        #region Camera Zoom:

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

        #endregion
    }
}


