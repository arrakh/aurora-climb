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

        private void Update()
        {
            slider.value = hand.Stamina;
            fill.color = barColorOverValue.Evaluate(slider.value);
        }
    }
}