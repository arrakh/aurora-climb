using System;
using AuroraClimb.Player;
using UnityEngine;
using UnityEngine.UI;

namespace AuroraClimb.UI
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private PlayerHand hand;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        [SerializeField] private Gradient barColorOverValue;
        [SerializeField] private float pixelWidthPerStaminaDuration = 4f;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            slider.value = hand.Stamina;
            fill.color = barColorOverValue.Evaluate(slider.value);

            var size = rectTransform.sizeDelta;
            size.x = hand.StaminaDrainDuration * pixelWidthPerStaminaDuration;
            rectTransform.sizeDelta = size;
        }
    }
}