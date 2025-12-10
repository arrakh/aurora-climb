using System.Collections.Generic;
using AuroraClimb.Items;
using AuroraClimb.Player;
using TMPro;
using UnityEngine;

namespace AuroraClimb.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private RectTransform holder;
        [SerializeField] private ItemSlot slotPrefab;
        [SerializeField] private RectTransform slotParent;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        private List<ItemSlot> spawnedSlots = new();

        private ItemSlot lastHoveredSlot;

        private bool open = false;

        private void OnEnable()
        {
            holder.gameObject.SetActive(open);
            playerInventory.OnInventoryUpdated += Display;
            Display();
        }

        private void OnDisable()
        {
            playerInventory.OnInventoryUpdated -= Display;
        }

        public void ToggleInventory()
        {
            open = !open;
            holder.gameObject.SetActive(open);

            if (open) descriptionText.text = "Hover over any item";
        }

        private void Display()
        {
            ClearChildren();
            
            foreach (var item in playerInventory.Items)
            {
                var slot = SpawnSlot();
                slot.Display(item, OnItemClick, OnItemHover);
            }

            var emptySlots = playerInventory.InventorySlotCount - playerInventory.Items.Count;
            for (int i = 0; i < emptySlots; i++)
            {
                var slot = SpawnSlot();
                slot.DisplayEmpty();
            }
        }

        private void OnItemClick(ItemSlot slot)
        {
            
        }

        private void OnItemHover(ItemSlot slot)
        {
            var data = slot.Data;

            var item = ItemDatabase.Get(data.id);

            descriptionText.text = $"<size=140%><b>{item.DisplayName}</b></size><size=20%>\n\n</size>{item.Description}";
            
            if (lastHoveredSlot != null) lastHoveredSlot.SetShowHighlight(false);
            lastHoveredSlot = slot;
            lastHoveredSlot.SetShowHighlight(true);
        }

        private ItemSlot SpawnSlot()
        {
            var slot = Instantiate(slotPrefab, slotParent);
            spawnedSlots.Add(slot);
            return slot;
        }

        private void ClearChildren()
        {
            foreach (var slot in spawnedSlots)
                Destroy(slot.gameObject);
            
            spawnedSlots.Clear();
        }
    }
}