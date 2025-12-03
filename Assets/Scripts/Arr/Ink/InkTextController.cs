using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Arr.Ink
{
    public class InkTextController : MonoBehaviour
    {
        [SerializeField] private InkStoryController inkStoryController;
        [SerializeField] private TextMeshProUGUI lineText;
        [SerializeField] private float letterPause;
        [SerializeField] private bool useHistory = false;

        private Coroutine typeCoroutine;
        private string targetText;
        private string historyText;
        private bool isTyping;
        
        public event Action OnTextFinished; 

        public bool IsTyping => isTyping;
        
        private void OnEnable() => inkStoryController.OnStoryText += OnText;
        private void OnDisable() => inkStoryController.OnStoryText -= OnText;

        private void OnText(string text)
        {
            Stop();
            targetText = text;
            typeCoroutine = StartCoroutine(TypeSentence());
        }

        public void Finish()
        {
            Stop();
            
            if (useHistory)
            {
                historyText += targetText;
                lineText.text = historyText;
            } 
            else lineText.text = targetText;

            OnTextFinished?.Invoke();
        }

        private void Stop()
        {
            if (typeCoroutine != null) StopCoroutine(typeCoroutine);
            isTyping = false;
        }

        IEnumerator TypeSentence()
        {
            isTyping = true;
            //lineText.text = historyText;
            lineText.text = String.Empty;
            string currentText = String.Empty;

            // This regex pattern will match any TMP tags.
            string pattern = @"<.*?>";

            string[] parts = Regex.Split(targetText, pattern);

            int currentIndex = 0;
            foreach (string part in parts)
            {
                if (Regex.IsMatch(part, pattern))
                {
                    // If the part is a TMP tag, append it whole without delay.
                    currentText += part;
                    lineText.text = historyText + currentText;
                }
                else
                {
                    // If the part is normal text, reveal it letter by letter.
                    foreach (char letter in part)
                    {
                        currentText += letter;
                        currentIndex++;
                        lineText.text = historyText + currentText;
                        yield return new WaitForSeconds(letterPause);
                    }
                }
            }
            
            Finish();
        }
    }
}