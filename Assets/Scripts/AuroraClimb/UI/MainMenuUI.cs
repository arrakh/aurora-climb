using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AuroraClimb.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Image darkBg;
        [SerializeField] private Button playButton, quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(OnPlay);
            quitButton.onClick.AddListener(OnQuit);
        }

        private void OnQuit()
        {
            Application.Quit();
        }

        private void Start()
        {
            darkBg.color = Color.black;
            darkBg.CrossFadeAlpha(0f, 2f, true);
        }

        private void OnPlay()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}