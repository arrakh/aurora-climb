using System;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerFogBox : MonoBehaviour
    {
        [SerializeField] private Vector2 heightRange;
        [SerializeField] private MeshRenderer fogRenderer;
        [SerializeField] private AnimationCurve fogCurve;
        private static readonly int Color = Shader.PropertyToID("_Color");

        private Color targetColor;
        
        private void Awake()
        {
            targetColor = fogRenderer.sharedMaterial.GetColor(Color);
        }

        [ExecuteAlways]
        private void Update()
        {
            var color = targetColor;
            var alpha = Mathf.InverseLerp(heightRange.x, heightRange.y, transform.position.y);
            color.a = fogCurve.Evaluate(alpha);
            //Debug.Log($"ALPHA: {alpha} | {color}");
            fogRenderer.sharedMaterial.SetColor(Color, color);
        }
    }
}