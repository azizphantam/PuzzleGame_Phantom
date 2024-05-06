#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;
using Watermelon;


public abstract class UICanvasAbstract : MonoBehaviour
{
    [Header("Canvas Info")]
    [SerializeField] Canvas mainCanvas;
    [SerializeField] RectTransform mainPanel;
    [SerializeField] CanvasScaler mainCanvasScaler;

    public Canvas Canvas => mainCanvas;
    public RectTransform Panel => mainPanel;
    public CanvasScaler CanvasScaler => mainCanvasScaler;
    protected float CanvasWidth { get; private set; }

    protected void Awake()
    {
        SetupUIForScreenRatio();
        Tween.NextFrame(delegate {
            CanvasWidth = Canvas.GetComponent<RectTransform>().sizeDelta.x;
        });
    }

    public abstract void Show();
    public abstract void Hide();

    protected void SetupUIForScreenRatio()
    {
        float screenRatio = Screen.width / (float)Screen.height;

        if (screenRatio > mainCanvasScaler.referenceResolution.x / mainCanvasScaler.referenceResolution.y)
        {
            mainCanvasScaler.matchWidthOrHeight = 1f;
        }
    }
}
