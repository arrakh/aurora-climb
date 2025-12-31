using System;
using System.Collections.Generic;
using AuroraClimb.Items;
using AuroraClimb.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AuroraClimb.UI
{
    public class InventoryPromptUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private ItemSlot slotPrefab;
        [SerializeField] private RectTransform holder;
        [SerializeField] private RectTransform slotParent;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button cancelButton;
        
        private List<ItemSlot> spawnedSlots = new();

        private ItemSlot lastHoveredSlot;
        private Action<ItemData> onItem;
        private Action onCancelled;

        private bool isOpen = false;

        public bool IsOpen => isOpen;
        
        private void Awake()
        {
            cancelButton.onClick.AddListener(Cancel);
        }

        private void Cancel()
        {
            onCancelled?.Invoke();
            SetShow(false);
        }

        private void SetShow(bool show)
        {
            isOpen = show;
            holder.gameObject.SetActive(show);
        }

        public void Display(Action<ItemData> onItemSelected, string prompt = "Select an item", Action onCancel= null)
        {
            SetShow(true);
            
            onItem = onItemSelected;
            onCancelled = onCancel;
            
            descriptionText.text = prompt;
            
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
            if (!isOpen) return;
            SetShow(false);
            
            onItem?.Invoke(slot.Data);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isOpen) Cancel(); 
        }
    }
}