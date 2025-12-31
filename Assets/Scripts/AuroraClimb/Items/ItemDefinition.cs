using UnityEngine;

namespace AuroraClimb.Items
{
    [CreateAssetMenu(menuName = "Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private bool canBeBurned = true;
        [SerializeField] private float staminaGainWhenBurned = 0.2f;

        public Sprite Icon => icon;

        public string Description => description;

        public string DisplayName => displayName;

        public string ID => id;
        
        public bool CanBeBurned => canBeBurned;

        public float StaminaGainWhenBurned => staminaGainWhenBurned;
    }
}