using System;
using Arr.Ink;
using UnityEngine;

namespace AuroraClimb.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private InkStoryController storyController;
        [SerializeField] private TextAsset inkStoryJson;

        private void Awake()
        {
            storyController.InitializeStory(inkStoryJson.text);
        }
    }
}