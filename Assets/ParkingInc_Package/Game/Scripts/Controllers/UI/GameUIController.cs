#pragma warning disable 649

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Watermelon;

public class GameUIController : UICanvasAbstract
{
    [SerializeField] RectTransform safeAreaPanel;

    [Header("Buttons")]
    [SerializeField] Button replayButton;
    [SerializeField] Button skipButton;

    [Header("Coins")]
    [SerializeField] Text levelText;
    [SerializeField] Text coinsAmountText;

    [Header("Dev Panel")]
    [SerializeField] RectTransform devPanel;

    new void Awake()
    {
        base.Awake();

        safeAreaPanel.anchoredPosition = Vector3.down * UIController.SafeAreaTopOffset;
    } 

    public override void Show(){}
    public override void Hide(){}

    public void ReplayButton()
    {

        if (!UITouchHandler.CanReplay) return;

        GameController.ReplayLevel();

        SetReplayButtonVisibility(false);

        GameAudioController.PlayButtonAudio();
    }

    public void SetCoinsAmount(int coinsAmount)
    {
        coinsAmountText.text = coinsAmount.ToString();
    }

    public void SetLevelText(int level)
    {
        levelText.text = "LEVEL " + (level + 1);
    }

    public void SkipButton()
    {
        GameAudioController.PlayButtonAudio();

        AdsManager.ShowRewardBasedVideo((hasWatched) => 
        {
            if (hasWatched)
            {
                GameController.TurnsAfterRewardVideo = 0;
                GameController.SkipLevel();
            }
        });
        
    }

    public void FirstLevelButton()
    {
        if (GameController.WinStage) return;
        GameController.FirstLevelDev();
    }

    public void NextLevelButton()
    {
        if (GameController.WinStage) return;
        //GameController.SkipLevel();
        LevelController.DestroyLevel();
        GameController.NextLevel(false);
    }

    public void PreviousLevel()
    {
        if (GameController.WinStage) return;
        GameController.PrevLevelDev();
    }

    public void HideButton()
    {
        devPanel.gameObject.SetActive(false);
    }

    public void SetReplayButtonVisibility(bool isShown)
    {
        if (isShown)
        {
            replayButton.image.rectTransform.DOAnchoredPosition(Vector3.up * 470f, 0.5f).SetEasing(Ease.Type.QuadOut);
        } else
        {
            replayButton.image.rectTransform.DOAnchoredPosition(new Vector2(200, 470), 0.5f).SetEasing(Ease.Type.QuadOut);
        }
    }

    public void SetSkipButtonVisibility(bool isShown)
    {
        if (isShown)
        {
            skipButton.image.rectTransform.DOAnchoredPosition(Vector3.up * 470f, 0.5f).SetEasing(Ease.Type.QuadOut);
        }
        else
        {
            skipButton.image.rectTransform.DOAnchoredPosition(new Vector2(-200, 470), 0.5f).SetEasing(Ease.Type.QuadOut);
        }
    }

    public void ShopButton()
    {
        GameAudioController.PlayButtonAudio();

        StoreUIController.OpenStore();
    }
}
