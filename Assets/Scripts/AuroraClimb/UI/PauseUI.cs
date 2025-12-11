using System;
using UnityEngine;

namespace AuroraClimb.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private RectTransform holder;
        
        private bool isPaused = false;
        
        private void Toggle()
        {
            isPaused = !isPaused;
            SetEnabled(isPaused);
        }

        private void SetEnabled(bool on)
        {
            holder.gameObject.SetActive(on);
            Time.timeScale = on ? 0f : 1f;
            
            Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = on;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) 
                || Input.GetKeyDown(KeyCode.Tab) 
                || Input.GetKeyDown(KeyCode.I)) Toggle();
        }
    }
}