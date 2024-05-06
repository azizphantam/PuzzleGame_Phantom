#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Watermelon;

public class UIController : MonoBehaviour
{

    private static UIController instance;

    [SerializeField] Image blackFadePanel;
    [SerializeField] Canvas blackFadeCanvas;

    private static Image BlackFadePanel => instance.blackFadePanel;
    private static Canvas BlackFadeCanvas => instance.blackFadeCanvas;

    [Space]
    [SerializeField] GameUIController gameUI;

    private static GameUIController GameUI => instance.gameUI;


    private static float safeAreaTopOffset = -1;
    public static float SafeAreaTopOffset {
        get {
            if (safeAreaTopOffset == -1) safeAreaTopOffset = Screen.height - Screen.safeArea.height;
            return safeAreaTopOffset;
        }
        
    }

    void Start()
    {
        instance = this;
        BlackFadeOut();
    }

    private void BlackFadeOut()
    {
        blackFadeCanvas.enabled = true;
        blackFadePanel.color = Color.black;
        blackFadePanel.DOFade(0, 0.5f).OnComplete(() => {
            blackFadeCanvas.enabled = false;
        });

    }

    public static void SetLevel(int level)
    {
        GameUI.SetLevelText(level);
    }

    public static void DoHidden(UnityAction action)
    {
        BlackFadeCanvas.enabled = true;

        BlackFadePanel.DOFade(1, 0.3f).OnComplete(() => {
            action();
            BlackFadePanel.DOFade(0, 0.3f).OnComplete(() => {
                BlackFadeCanvas.enabled = false;
            });
        });
    }

    public static void SetReplayButtonVisibility(bool isShown)
    {
        GameUI.SetReplayButtonVisibility(isShown);
    }

    public static void SetSkipButtonVisibility(bool isShown)
    {
        GameUI.SetSkipButtonVisibility(isShown);
    }
}
