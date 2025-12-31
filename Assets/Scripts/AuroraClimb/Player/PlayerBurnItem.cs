using AuroraClimb.Controllers;
using AuroraClimb.Items;
using AuroraClimb.UI;
using UnityEngine;

namespace AuroraClimb.Player
{
    public class PlayerBurnItem : MonoBehaviour
    {
        [SerializeField] private InventoryPromptUI inventoryPromptUi;
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private PlayerStateController stateController;
        [SerializeField] private PlayerMovementNew movement;
        [SerializeField] private GlobalFlagsController globalFlagsController;

        private const string HAS_BURNED = "hasBurned";

        public bool HasBurnable()
        {
            foreach (var item in inventory.Items)
            {
                var def = ItemDatabase.Get(item.id);
                if (def.CanBeBurned) return true;
            }
            
            return false;
        }

        public void PromptBurnItem()
        {
            if (inventoryPromptUi.IsOpen) return;
            stateController.SetIsInDialogue(true);
            inventoryPromptUi.Display(OnSelected, "Select an item to Burn", OnCancelled);
        }

        private void OnCancelled()
        {
            stateController.SetIsInDialogue(false);
        }

        private void OnSelected(ItemData item)
        {
            var def = ItemDatabase.Get(item.id);

            if (!def.CanBeBurned)
            {
                PromptBurnItem();
                return;
            }
            
            inventory.TryRemoveItem(item);
            var totalStamina = def.StaminaGainWhenBurned * item.amount;
            movement.AddMaxStamina(totalStamina);
            globalFlagsController.AddFlag($"{HAS_BURNED}-{item.id}");
            stateController.SetIsInDialogue(false);
        }
    }
}