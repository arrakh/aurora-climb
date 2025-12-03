using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Arr.Ink
{
    public class InkChoiceController : MonoBehaviour
    {
        [FormerlySerializedAs("inkController")] [SerializeField] private InkStoryController inkStoryController;
        [SerializeField] private InkChoiceElement choicePrefab;
        [SerializeField] private RectTransform choiceParent;

        private void OnEnable()
        {
            inkStoryController.OnStoryChoices += OnChoices;
            inkStoryController.OnStoryText += OnText;
        }

        private void OnDisable()
        {
            inkStoryController.OnStoryChoices -= OnChoices;
            inkStoryController.OnStoryText -= OnText;
        }

        private void OnText(string _)
        {
            choiceParent.gameObject.SetActive(false);
        }

        private void OnChoices(List<Choice> choices)
        {
            choiceParent.gameObject.SetActive(true);

            ClearChildren();

            foreach (var choice in choices)
            {
                var choiceElement = Instantiate(choicePrefab, choiceParent, false);
                choiceElement.Display(choice.text, choice.index, OnChoicePressed);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(choiceParent);
        }

        private void OnChoicePressed(InkChoiceElement element)
        {
            inkStoryController.ChooseChoice(element.Index);
        }

        private void ClearChildren()
        {
            foreach (Transform children in choiceParent)
                Destroy(children.gameObject);
        }
    }
}