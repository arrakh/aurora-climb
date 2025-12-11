using Arr.Ink;
using AuroraClimb.Items;
using AuroraClimb.Player;
using Ink.Runtime;
using UnityEngine;

namespace AuroraClimb.Controllers
{
    public class StoryFunctionsController : MonoBehaviour
    {
        [SerializeField] private InkStoryController inkStoryController;
        [SerializeField] private GlobalFlagsController globalFlags;
        [SerializeField] private PlayerInventory inventory;

        private Story story;

        private void Awake()
        {
            inkStoryController.OnStoryInitialized += OnStoryInitialized;
        }

        private void OnStoryInitialized(Story obj)
        {
            story = obj;

            InkFunctionAttribute.BindWithTypes(new []
                {  
                    typeof(StoryFunctionsController)
                }, 
                method =>
                {
                    object Del(object[] args) => method.Invoke(this, args);
                    story.BindExternalFunctionGeneral(method.Name, Del);
                });
        }

        [InkFunction]
        private bool HasGlobalFlag(string id) => globalFlags.HasFlag(id);

        [InkFunction] 
        private void AddGlobalFlag(string id) => globalFlags.AddFlag(id);

        [InkFunction] 
        private void RemoveGlobalFlag(string id) => globalFlags.RemoveFlag(id);

        [InkFunction]
        private void AddInventoryCount(int amount) => inventory.AddSlotCount(amount);

        [InkFunction]
        private int GetInventoryCount() => inventory.InventorySlotCount;

        [InkFunction]
        private int GetTotalItemCount() => inventory.Items.Count;

        [InkFunction]
        private int GetItemCount(string id) => inventory.GetItemCount(id);

        [InkFunction]
        private bool HasItem(string id) => inventory.HasItem(new ItemData(id, 1));

        [InkFunction]
        private void AddItem(string id) => inventory.AddItem(new ItemData(id, 1));

        [InkFunction]
        private void AddItemWithAmount(string id, int amount) => inventory.AddItem(new ItemData(id, amount));

        [InkFunction]
        private void RemoveItem(string id) => inventory.TryRemoveItem(new ItemData(id, 1));

        [InkFunction]
        private void RemoveItemWithAmount(string id, int amount) => inventory.TryRemoveItem(new ItemData(id, amount));
    }
}