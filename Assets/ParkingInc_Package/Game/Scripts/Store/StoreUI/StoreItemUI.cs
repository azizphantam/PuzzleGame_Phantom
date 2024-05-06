using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    //Store Module v0.9.2
    public class StoreItemUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] Color outlineColorNeutral;
        [SerializeField] Color outlineColorSelected;
        [SerializeField] Color lockedImageColor;

        [Header("References")]
        [SerializeField] Image productImage;
        [SerializeField] Image outlineImage;
        [SerializeField] GameObject questionMarkObject;

        public StoreProduct ProductRef { get; private set; }

        public void Init(StoreProduct product, bool isSelected)
        {
            ProductRef = product;

            UpdateItem(isSelected);
        }

        public void UpdateItem(bool isSelected)
        {
            if (ProductRef == null)
            {
                Debug.LogError("Store Items is not initialized.");
                return;
            }


            outlineImage.color = isSelected ? outlineColorSelected : outlineColorNeutral;

            if (ProductRef.IsUnlocked())
            {
                productImage.sprite = ProductRef.UnlockedIcon;
                productImage.color = Color.white;
            }
            else
            {
                productImage.sprite = ProductRef.LockedIcon;
                productImage.color = lockedImageColor;
            }
        }

        public void OnClick()
        {
            if (ProductRef.IsUnlocked())
            {
                StoreController.TryToSelectProduct(ProductRef.ID);
            }
        }

        public void SetHighlightState(bool active)
        {
            outlineImage.color = active ? outlineColorSelected : outlineColorNeutral;
        }
    }
}