#pragma warning disable 649

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class WatermelonToggle : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] bool value = true;
    [SerializeField] bool interactable = true;

    [Header("Sprites")]
    [SerializeField] Sprite trueSprite;
    [SerializeField] Sprite falseSprite;

    [Header("Event")]
    [SerializeField] bool clickOnly = false;
    [SerializeField] WatermelonToggleEvent onValueChange = new WatermelonToggleEvent();
    
    private Image image;

    public bool Interactable { get => interactable; set => interactable = value; }

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = value ? trueSprite : falseSprite;
    }

    public bool Value
    {
        get => value; 
        set
        {
            this.value = value;
            if (!clickOnly) onValueChange.Invoke(arg0: value);

            image.sprite = value ? trueSprite : falseSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            Value = !Value;
            if (clickOnly) onValueChange.Invoke(arg0: Value);
        }
    }
}

[System.Serializable]
public class WatermelonToggleEvent : UnityEvent<bool>{}
