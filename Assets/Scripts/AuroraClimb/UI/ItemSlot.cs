using System;
using AuroraClimb.Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AuroraClimb.UI
{
    public class ItemSlot : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private RectTransform contentGroup;
        [SerializeField] private RectTransform highlightedGroup;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Button button;

        private ItemData currentData;
        private ItemDefinition currentDefinition;

        private Action<ItemSlot> onItemClick;
        private Action<ItemSlot> onItemHover;
        private bool isEmpty;

        public ItemData Data => currentData;
        public ItemDefinition Definition => currentDefinition;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (isEmpty) return;
            onItemClick?.Invoke(this);
        }

        public void Display(ItemData data, Action<ItemSlot> onClick, Action<ItemSlot> onHover)
        {
            currentData = data;
            currentDefinition = ItemDatabase.Get(data.id);

            onItemClick = onClick;
            onItemHover = onHover;

            contentGroup.gameObject.SetActive(true);
            isEmpty = false;
            
            amountText.text = data.amount <= 1 ? "" : data.amount.ToString();
            icon.sprite = currentDefinition.Icon;
        }

        public void DisplayEmpty()
        {
            contentGroup.gameObject.SetActive(false);
            isEmpty = true;
        }

        public void SetShowHighlight(bool show) => highlightedGroup.gameObject.SetActive(show);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isEmpty) return;
            onItemHover?.Invoke(this);
        }
    }
}