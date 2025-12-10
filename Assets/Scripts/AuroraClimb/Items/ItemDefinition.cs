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

        public Sprite Icon => icon;

        public string Description => description;

        public string DisplayName => displayName;

        public string ID => id;
    }
}