using PerfectMazeDFS_Pointers;
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

        private void UpdateNewPosition()
        {
            transform.position = newCameraPos;
        }

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

        private void SetMovePointersState(int moveIndex, bool isMovePointerDown)
        {
            this.moveIndex = moveIndex;
            this.isMovePointerDown = isMovePointerDown;

            /*        Debug.Log("Index = " + this.index);
                    Debug.Log("PointerDown = " + this.isPointerDown);*/
        }

        private void SetZoomPointersState(int zoomIndex, bool isZoomPointerDown)
        {
            this.zoomIndex = zoomIndex;
            this.isZoomPointerDown = isZoomPointerDown;
        }

        private void MoveCameraWithPointers() // 0 - Right, 1 - Left, 2 - Up, 3 - Down
        {
            if (!isMovePointerDown)
                return;

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
            if (!isZoomPointerDown)
                return;

            if (zoomIndex == 0)
                ZoomIn();
            else if (zoomIndex == 1)
                ZoomOut();
        }

        #endregion

        #region Movement:

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

        #region Zoom:

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

        #region PC_Controls:
        private void ZoomCameraControls_PC()
        {
            if (Input.GetKey(KeyCode.E))
                ZoomIn();

            else if (Input.GetKey(KeyCode.Q))
                ZoomOut();
        }

        private void MoveCameraControls_PC()
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

        #endregion

    }
}


