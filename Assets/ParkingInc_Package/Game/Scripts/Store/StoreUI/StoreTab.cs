using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    //Store Module v0.9.2
    public class StoreTab : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] StoreProductType type;
        public StoreProductType Type => type;

        [Space(5f)]
        [SerializeField] Color frontSelectedColor = Color.white;
        [SerializeField] Color frontNeutralColor = Color.white;

        [SerializeField] Color backSelectedColor = Color.white;
        [SerializeField] Color backNeutralColor = Color.white;


        [Header("References")]
        [SerializeField] Image backPanelImage;
        [SerializeField] Text frontPanelText;

        public void SetActiveState(bool active)
        {
            frontPanelText.color = active ? frontSelectedColor : frontNeutralColor;
            backPanelImage.color = active ? backSelectedColor : backNeutralColor;
        }

        public void OnClick()
        {
            StoreUIController.OnTabPressed(type);
        }
    }
}