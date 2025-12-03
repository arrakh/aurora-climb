using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arr.Ink
{
    public class InkChoiceElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform holder;
        
        public int Index => index;
        
        private int index;
        private Action<InkChoiceElement> onPressed;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            onPressed?.Invoke(this);
        }

        public void Display(string text, int choiceIndex, Action<InkChoiceElement> onChoicePressed)
        {
            choiceText.text = text;
            index = choiceIndex;
            onPressed = onChoicePressed;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(holder);
        }
    }
}