using System;
using AuroraClimb.UI;
using UnityEngine;
using UnityEngine.XR;

namespace AuroraClimb.Player
{
    public class PlayerClimbController : MonoBehaviour
    {
        [SerializeField] private CursorUI cursorUi;
        [SerializeField] private Camera cam;
        [SerializeField] private PlayerHand leftHand, rightHand;
        [SerializeField] private float interactionDistance;
        [SerializeField] private LayerMask climbableLayer;

        private bool hasClimbableHit = false;
        private RaycastHit lastClimbableHit;

        private void Update()
        {
            DetectClimbable();
            GrabInputUpdate();
        }

        private void GrabInputUpdate()
        {
            if (Input.GetMouseButtonUp(0)) leftHand.Release();
            if (Input.GetMouseButtonUp(1)) rightHand.Release();
            
            if (!hasClimbableHit) return;

            if (Input.GetMouseButtonDown(0)) leftHand.Grab(lastClimbableHit);
            if (Input.GetMouseButtonDown(1)) rightHand.Grab(lastClimbableHit);
        }

        private void DetectClimbable()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (!Physics.Raycast(ray, out var hit, interactionDistance, climbableLayer))
            {
                hasClimbableHit = false;
                cursorUi.SetCursor(CursorUI.CursorType.Normal);
                return;
            }

            hasClimbableHit = true;
            cursorUi.SetCursor(CursorUI.CursorType.Climb);
            lastClimbableHit = hit;
        }
    }
}