using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector2 lookSpeed = new Vector2(200f, 200f);
        [SerializeField] private Transform playerBody;
        [SerializeField] private Vector2 pitchClamp = new(-85f, 85f);
        [SerializeField] private PlayerStateController stateController;

        private float pitch;
        private bool canControlCamera = true;

        private void Start()
        {
            stateController.OnStateChanged += OnStateChanged;
            
            if (cam == null) cam = Camera.main;
            if (playerBody == null) playerBody = transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnStateChanged(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.FreeMovement:
                    canControlCamera = true;
                    
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case PlayerState.InDialogue:
                    canControlCamera = false;

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
            }
        }

        private void Update()
        {
            if (canControlCamera) ControlCameraUpdate();
        }

        private void ControlCameraUpdate()
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed.x * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed.y * Time.deltaTime;

            playerBody.Rotate(Vector3.up * mouseX);

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);

            cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

}