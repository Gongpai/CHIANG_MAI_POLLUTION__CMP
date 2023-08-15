using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    public class Camera_Controller : MonoBehaviour
    {
        [SerializeField] private int MaxGrid;
        [SerializeField] private MinMax LimitZoom;
        [SerializeField] private MinMax LimitXRotation;
        [SerializeField] private float MovementScale;
        [SerializeField] private float Boundary = 50;
        [SerializeField] private float ZoomScale;
        [SerializeField] private float RotationScale;
        [SerializeField] private CinemachineVirtualCamera CM_Vcam;
        
        private Cinemachine3rdPersonFollow CM_3rdPF;

        private float currentHorizontalMove;
        private float currentVerticalMove;
        private Vector2 ScreenResolution;
        private float currentLeft_RightRotation;
        private float currentUp_DownRotation;
        private Vector2 currentMouseRotation;
        private float currentZoom;
        private bool IsCanRotWithMouse = false;

        private void Start()
        {
            CM_3rdPF = CM_Vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        public void Update()
        {
            //H_move
            float h_totalmove = (currentHorizontalMove * MovementScale) * Time.deltaTime;
            float hMovement = h_totalmove;
            transform.position += new Vector3(transform.right.x, 0, transform.right.z) * hMovement;
            //V_move
            float v_totalmove = (currentVerticalMove * MovementScale) * Time.deltaTime;
            float vMovement = v_totalmove;
            transform.position += (transform.forward + transform.up) * vMovement;
            
            //Move with Screen edge
            float screen_totalmove = MovementScale * Time.deltaTime;
            ScreenResolution = new Vector2(Screen.width, Screen.height);
            if (!PointerOverUIElement.OnPointerOverUIElement())
            {
                if (Input.mousePosition.x > ScreenResolution.x - Boundary &&
                    Input.mousePosition.x <= ScreenResolution.x)
                {
                    transform.position += new Vector3(transform.right.x, 0, transform.right.z) * screen_totalmove;
                }

                if (Input.mousePosition.x < 0 + Boundary && Input.mousePosition.x >= 0)
                {
                    transform.position -= new Vector3(transform.right.x, 0, transform.right.z) * screen_totalmove;
                }

                if (Input.mousePosition.y > ScreenResolution.y - Boundary &&
                    Input.mousePosition.y <= ScreenResolution.y)
                {
                    transform.position += (transform.forward + transform.up) * screen_totalmove; // move on +Z axis
                }

                if (Input.mousePosition.y < 0 + Boundary && Input.mousePosition.y >= 0)
                {
                    transform.position -= (transform.forward + transform.up) * screen_totalmove; // move on -Z axis
                }
            }

            //Limit Horizontal Movement
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -MaxGrid, MaxGrid), 0, transform.position.z);

            //Limit Vertical Movement
            transform.position = new Vector3(transform.position.x, 0, Mathf.Clamp(transform.position.z, -MaxGrid, MaxGrid));

            //---------------------------------- Zoom Cam -----------------------------------//
            ////Zoom
            float zoomCam = (currentZoom * ZoomScale) * Time.deltaTime;
            //Limit Zoom Camera
            CM_3rdPF.CameraDistance += zoomCam;
            CM_3rdPF.CameraDistance = Mathf.Clamp(CM_3rdPF.CameraDistance, LimitZoom.min, LimitZoom.max);

            //--------------------------------------- Rotation Cam -----------------------------------------------//
            //Rotation Up/Down
            float verticalRot = currentUp_DownRotation * RotationScale * Time.deltaTime;
            float horizontalRot = currentLeft_RightRotation * RotationScale * Time.deltaTime;
            transform.Rotate(Vector3.right, verticalRot);
            transform.Rotate(Vector3.up, -horizontalRot, Space.World);
            
            //--------------------------------------- Rotation Cam with mouse-----------------------------------------------//
            if (IsCanRotWithMouse)
            {
                //print("CanRot");
                //Rotation Up/Down
                float m_verticalRot = currentMouseRotation.y * RotationScale * Time.deltaTime;
                float m_horizontalRot = currentMouseRotation.x * RotationScale * Time.deltaTime;
                transform.Rotate(Vector3.right, -m_verticalRot);
                transform.Rotate(Vector3.up, m_horizontalRot, Space.World);
            }
            
            //Limit Rot
            float angle_y = transform.localEulerAngles.y;
            float angle_x = Mathf.Clamp(transform.localEulerAngles.x, LimitXRotation.min, LimitXRotation.max);
            transform.localEulerAngles = new Vector3(angle_x, angle_y, 0);
        }

        public void OnHorizontalMoveCamera(InputAction.CallbackContext value)
        {
            //print("Moveeeeeeeeeeeeeeeeeeeeeee : ");
            currentHorizontalMove = value.ReadValue<float>();

        }

        public void OnVerticalMoveCamera(InputAction.CallbackContext value)
        {
            //print("Moveeeeeeeeeeeeeeeeeeeeeee : ");
            currentVerticalMove = value.ReadValue<float>();
        }

        public void OnRotationLeft_RightCamera(InputAction.CallbackContext value)
        {
            //print("Rotttttttttttttttttttttt : " + value.ReadValue<float>());
            currentLeft_RightRotation = value.ReadValue<float>();
        }
        
        public void OnRotationUp_DownCamera(InputAction.CallbackContext value)
        {
            //print("Rotttttttttttttttttttttt : " + value.ReadValue<float>());
            currentUp_DownRotation = value.ReadValue<float>();
        }

        public void OnRotationWithMouse(InputAction.CallbackContext value)
        {
            //print("vec : " + value.ReadValue<Vector2>());
            currentMouseRotation = value.ReadValue<Vector2>();
        }

        public void CanRotationWithMouse(InputAction.CallbackContext value)
        {
            if(value.performed)
                IsCanRotWithMouse = !IsCanRotWithMouse;
            //print(IsCanRotWithMouse + " : " + value);
        }

        public void OnZoomCamera(InputAction.CallbackContext value)
        {
            //print("Zoommmmmmmmmmmmmmmmmmm : " + value.ReadValue<float>());
            currentZoom = value.ReadValue<float>();
        }
    }
}
