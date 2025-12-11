using System;
using System.Collections.Generic;
using AuroraClimb.Items;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int inventorySlotCount = 4;

        private List<ItemData> items = new();

        public event Action OnInventoryUpdated;
        
        public int InventorySlotCount => inventorySlotCount;

        public IReadOnlyList<ItemData> Items => items;

        public void AddSlotCount(int amount)
        {
            inventorySlotCount += amount;
            OnInventoryUpdated?.Invoke();
        }

        public bool HasItem(ItemData toCheck)
        {
            int similarItemCount = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.id.Equals(toCheck.id, StringComparison.InvariantCultureIgnoreCase))
                    similarItemCount += item.amount;
            }

            return similarItemCount > 0;
        }

        public int GetItemCount(string id)
        {
            int total = 0;
            
            foreach (var item in items)
                if (item.id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                    total += item.amount;

            return total;
        }

        public void AddItem(ItemData toAdd)
        {
            bool foundSimilar = false;
            int index = 0;
            
            var ignore = StringComparison.InvariantCultureIgnoreCase;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.id.Equals(toAdd.id, ignore)) continue;
                foundSimilar = true;
                index = i;
                break;
            }

            if (!foundSimilar)
            {
                items.Add(toAdd);
                OnInventoryUpdated?.Invoke();
                return;
            }
            
            //Found similar item
            var existing = items[index];
            var finalItem = new ItemData(existing.id, existing.amount + toAdd.amount);
            items[index] = finalItem;
            OnInventoryUpdated?.Invoke();
        }
        
        public bool TryRemoveItem(ItemData toRemove)
        {
            var ignore = StringComparison.InvariantCultureIgnoreCase;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item.id.Equals(toRemove.id, ignore)) continue;

                if (item.amount < toRemove.amount) return false; //not enough

                int remaining = item.amount - toRemove.amount;

                if (remaining > 0) items[i] = new ItemData(item.id, remaining);
                else items.RemoveAt(i);

                OnInventoryUpdated?.Invoke();
                return true;
            }

            return false; //cant find
        }
    }
}