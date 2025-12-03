using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arr.Utilities;
using Ink.Runtime;
using UnityEngine;

namespace Arr.Ink
{
    public class InkStoryController : MonoBehaviour
    {
        [SerializeField] private InkTextController textController;
        [SerializeField] private bool waitForContinueWhenStoryEnds = false;
        
        private TaskCompletionSource<int> storyChoiceIndexTcs = new();
        private TaskCompletionSource<bool> storyContinueTcs = new();

        private Story story;

        public event Action<List<Choice>> OnStoryChoices;
        public event Action<string> OnStoryText; 
        public event Action<List<string>> OnStoryTags;
        public event Action<Story> OnStoryInitialized;
        public event Action OnStoryDone;

        public void InitializeStory(string jsonText, string startAtPath = null)
        {
            story = new Story(jsonText);
            
            OnStoryInitialized?.Invoke(story);

            if (!String.IsNullOrEmpty(startAtPath))
                story.ChoosePathString(startAtPath);
        }

        public void StartStory(string startAtPath = null)
        {
            StartCoroutine(StartStoryCoroutine(startAtPath));
        }

        public IEnumerator StartStoryCoroutine(string startAtPath = null)
        {
            if (!String.IsNullOrEmpty(startAtPath))
                story.ChoosePathString(startAtPath);

            while (story.canContinue)
            {
                var nextLine = story.Continue();

                if (string.IsNullOrWhiteSpace(nextLine)) continue;

                OnStoryText?.Invoke(nextLine);
                OnStoryTags?.Invoke(story.currentTags);
                Debug.Log("CONTINUING STORY");

                yield return new WaitUntil(() => !textController.IsTyping);

                if (story.currentChoices.Count > 0)
                {
                    OnStoryChoices?.Invoke(story.currentChoices);
                    //Task.Run(() => storyChoiceIndexTcs.Task);
                    yield return storyChoiceIndexTcs.WaitUntilCompleted();
                    var choiceIndex = storyChoiceIndexTcs.Task.Result;

                    story.ChooseChoiceIndex(choiceIndex);
                    story.Continue();
                }
                else
                {
                    //Task.Run(() => storyContinueTcs.Task);
                    yield return storyContinueTcs.WaitUntilCompleted();
                }

                storyChoiceIndexTcs = new();
                storyContinueTcs = new();
                Debug.Log($"END OF STORY, CAN CONTINUE? {story.canContinue}");
            }

            Debug.Log("STORY DONE!!!!");
            if (waitForContinueWhenStoryEnds)
            {
                yield return storyContinueTcs.WaitUntilCompleted();
                OnStoryDone?.Invoke();
            } else OnStoryDone?.Invoke();
        }

        public void ChooseChoice(int index) => storyChoiceIndexTcs.TrySetResult(index);

        public void Continue()
        {
            storyContinueTcs.TrySetResult(true);
        }

        public void ContinueOrFinish()
        {
            if (textController.IsTyping) textController.Finish();
            else Continue();
        }
    }
}
