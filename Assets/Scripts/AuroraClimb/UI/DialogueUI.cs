using System;
using Arr.Ink;
using AuroraClimb.Player;
using UnityEngine;

namespace AuroraClimb.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private RectTransform holder;
        [SerializeField] private InkStoryController storyController;

        private void Awake()
        {
            storyController.OnStoryText += OnStoryText;
            storyController.OnStoryDone += OnStoryDone;
            holder.gameObject.SetActive(false);
        }

        private void OnStoryDone()
        {
            holder.gameObject.SetActive(false);
        }

        private void OnStoryText(string obj)
        {
            holder.gameObject.SetActive(true);
        }
    }
}