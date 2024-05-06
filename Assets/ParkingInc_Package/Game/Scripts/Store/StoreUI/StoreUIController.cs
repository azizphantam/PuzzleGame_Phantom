#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Watermelon
{
    //Store Module v0.9.1
    [HelpURL("https://docs.google.com/document/d/1SS9_U59ACe1kSrd2nxSmbqvWtTu6Uv1SQzcyocQLZB8/edit")]
    public class StoreUIController : MonoBehaviour
    {
        private static StoreUIController instance;
        public static readonly string STORE_ITEM_POOL_NAME = "StoreItem";
        public static readonly string PAGES_POOL_NAME = "StorePage";
        public static readonly string PAGE_DOT_POOL_NAME = "PageDot";

        [Header("Settings")]
        [SerializeField] float swipeThereshold = 0.2f;
        private static float SwipeThereshold => instance.swipeThereshold;

        [SerializeField] StoreAnimationConfig animationConfig;
        public static StoreAnimationConfig AnimationConfig => instance.animationConfig;

        [Header("References")]
        [SerializeField] GameObject storeItemPrefab;
        private static GameObject StoreItemPrefab => instance.storeItemPrefab;
        [SerializeField] GameObject pageDotPrefab;
        private static GameObject PageDotPrefab => instance.pageDotPrefab;
        [SerializeField] GameObject pagePrefab;
        private static GameObject PagePrefab => instance.pagePrefab;


        [Space(5f)]
        [SerializeField] Canvas storeCanvas;
        private static Canvas StoreCanvas => instance.storeCanvas;

        [SerializeField] Image storeBackground;
        private static Image StoreBackground => instance.storeBackground;

        [SerializeField] RectTransform shineTransform;
        private static RectTransform ShineTransform => instance.shineTransform;

        [SerializeField] CanvasGroup shineCanvasGroup;
        private static CanvasGroup ShineCanvasGroup => instance.shineCanvasGroup;

        [SerializeField] RectTransform storePanel;
        private static RectTransform StorePanel => instance.storePanel;

        [SerializeField] CanvasGroup storePanelCanvasGroup;
        private static CanvasGroup StorePanelCanvasGroup => instance.storePanelCanvasGroup;

        //private static Transform StoreObjectsHolder => instance.storeObjectsHolder;
        //[SerializeField] Transform storeObjectsHolder;
        //private static Transform StoreObjectsHolder => instance.storeObjectsHolder;
        [SerializeField] RectTransform pagesHolderTransform;
        private static RectTransform PagesHolderTransform => instance.pagesHolderTransform;
        [SerializeField] Transform pageDotsHolderTransform;
        private static Transform PageDotsHolderTransform => instance.pageDotsHolderTransform;
        //[SerializeField] Transform previewHolderTransform;
        //private static Transform PreviewHolderTransform => instance.previewHolderTransform;


        private static Image PreviewImage => instance.previewImage;
        [SerializeField] Image previewImage;
        

        private static RectTransform StoreCanvasRect => instance.storeCanvasRect;
        [Space(5f), SerializeField] RectTransform storeCanvasRect;
        private static GameObject UnlockRandomButtonObject => instance.unlockRandomButtonObject;
        [SerializeField] GameObject unlockRandomButtonObject;
        private static GameObject GetCoinsButtonObject => instance.getCoinsButtonObject;
        [SerializeField] GameObject getCoinsButtonObject;
        private static Text UnlockRandomPriceText => instance.unlockRandomPriceText;
        [SerializeField] Text unlockRandomPriceText;
        private static Text CoinsForAdsText => instance.coinsForAdsText;
        [SerializeField] Text coinsForAdsText;
        private static Transform CoinsPanelTransform => instance.coinsPanelTransform;
        [SerializeField] Transform coinsPanelTransform;
        private static Text CoinsAmountText => instance.coinsAmountText;
        [SerializeField] Text coinsAmountText;
        private static RectTransform SafeArea => instance.safeArea;
        [SerializeField] RectTransform safeArea;

        private static RectTransform CloseButtonTransform => instance.closeButtonTransform;
        [SerializeField] RectTransform closeButtonTransform;
        private static Button CloseButton => instance.closeButton;
        [SerializeField] Button closeButton;
        private static Button BackgroundCloseButton => instance.backgroundCloseButton;
        [SerializeField] Button backgroundCloseButton;

        [Space(5f)]
        [SerializeField] List<StoreTab> tabsList = new List<StoreTab>();
        private static List<StoreTab> TabsList => instance.tabsList;

        private static Dictionary<StorePageName, List<StoreProduct>> productsByGroupDictionary;
        private static List<Image> pageDotsImagesList = new List<Image>();
        private static List<StorePage> pagesList = new List<StorePage>();
        private static List<StoreProduct> skinsOnCurrentPageList = new List<StoreProduct>();
        private static List<int> closedSkinsOnCurrentPageIndexes = new List<int>();
        private static List<int> pagesAmountForEachProductType = new List<int>();

        private static HorizontalLayoutGroup pageDotsLayout;
        private static Pool storeItemPool;
        private static Pool pagesPool;
        private static Pool pageDotsPool;
        private static CanvasScaler canvasScalerRef;

        private static Vector3 pagesHolderLocalPosition;
        public static StoreProductType CurrentProductType { get; private set; }

        public static bool CanInteract => !isAnimationRunning && !previewAnimationPlaying;

        private static bool isAnimationRunning;
        private static float canvasWidth;
        private static int currentPageIndex;
        private static int pagesAmount;

        private void Awake()
        {
            instance = this;
            canvasScalerRef = storeCanvasRect.GetComponent<CanvasScaler>();

            //ClearPreviewObjects();
            SetupUIForScreenRatio();

            canvasWidth = canvasScalerRef.referenceResolution.x;
            pageDotsLayout = pageDotsHolderTransform.GetComponent<HorizontalLayoutGroup>();
            storeItemPool = PoolManager.AddPool(new PoolSettings(STORE_ITEM_POOL_NAME, storeItemPrefab, 10, true));
            pagesPool = PoolManager.AddPool(new PoolSettings(PAGES_POOL_NAME, pagePrefab, 3, true, pagesHolderTransform));
            pageDotsPool = PoolManager.AddPool(new PoolSettings(PAGE_DOT_POOL_NAME, pageDotPrefab, 3, true, pageDotsHolderTransform));


            CloseButton.enabled = false;
            BackgroundCloseButton.enabled = false;
            CloseButtonTransform.localScale = Vector3.zero;
        }

        //private void ClearPreviewObjects()
        //{
        //    if (pagesHolderTransform.childCount > 0)
        //    {
        //        for (int i = 0; i < pagesHolderTransform.childCount; i++)
        //        {
        //            if (pagesHolderTransform.GetChild(i).name.Contains("[Page Preview]"))
        //            {
        //                Destroy(pagesHolderTransform.GetChild(i).gameObject);
        //            }
        //        }
        //    }
        //
        //    ClearOldPreviewModels();
        //}

        private void Start()
        {
            pagesAmountForEachProductType = StoreController.Database.GetPagesAmountPerProducts();

            StoreController.OnProductSelected += OnNewSkinSelected;

            SafeArea.anchoredPosition = Vector3.down * UIController.SafeAreaTopOffset;
        }

        private void SetupUIForScreenRatio()
        {
            float screenRatio = Screen.width / (float)Screen.height;

            if (screenRatio > canvasScalerRef.referenceResolution.x / canvasScalerRef.referenceResolution.y)
            {
                canvasScalerRef.matchWidthOrHeight = 1f;
            }
        }

        public static void ChangePreview(StoreProduct product)
        {
            //if (changePreviewTween != null && !changePreviewTween.isCompleted) changePreviewTween.Kill();
            if (previewAnimationPlaying)
            {
                shouldKillPreviewAnimation = true;
                Tween.NextFrame(() => {
                    shouldKillPreviewAnimation = false;
                    instance.StartCoroutine(ChangePreviewCoroutine(product));
                });
            } else
            {
                instance.StartCoroutine(ChangePreviewCoroutine(product));
            }

            
        }


        private static bool previewAnimationPlaying = false;

        private static bool shouldKillPreviewAnimation = false;

        private static IEnumerator ChangePreviewCoroutine(StoreProduct product)
        {
            /*changePreviewTween = PreviewImage.DOFade(0, 0.2f).OnComplete(() => {
                changePreviewTween = StoreBackground.DOColor(product.StoreBackgroundColor, 0.3f).OnComplete(() => {
                    PreviewImage.sprite = product.PreviewSprite;
                    changePreviewTween = PreviewImage.DOFade(1, 0.2f).OnComplete(() =>
                    {
                        changePreviewTween = null;
                    });
                });
            });*/

            previewAnimationPlaying = true;

            yield return DoFade(PreviewImage.color.a, 0, 0.15f, (float alpha) => PreviewImage.color = PreviewImage.color.SetAlpha(alpha));

            if (shouldKillPreviewAnimation) yield break;

            yield return DoColor(StoreBackground.color, product.StoreBackgroundColor, 0.15f, (Vector4 color) => StoreBackground.color = color);

            if (shouldKillPreviewAnimation) yield break;

            PreviewImage.sprite = product.PreviewSprite;

            yield return DoFade(0, 1, 0.15f, (float alpha) => PreviewImage.color = PreviewImage.color.SetAlpha(alpha));

            if (shouldKillPreviewAnimation) yield break;

            previewAnimationPlaying = false;
        }

        public static IEnumerator DoFade(float start, float end, float duration, UnityAction<float> action)
        {

            float time = 0f;
            do
            {
                yield return null;

                if (shouldKillPreviewAnimation) break;

                time += Time.deltaTime;
                action(start + (end - start) * (time / duration));

            } while (time < duration);

            action(end);
        }

        public static IEnumerator DoColor(Vector4 start, Vector4 end, float duration, UnityAction<Vector4> action)
        {
            float time = 0f;
            do
            {
                yield return null;

                if (shouldKillPreviewAnimation) break;

                time += Time.deltaTime;
                action(start + (end - start) * (time / duration));

            } while (time <= duration);

            action(end);
        }

        public static void OpenStore()
        {
            CoinsAmountText.text = GameController.CoinsCount.ToString();
            StoreCanvas.enabled = true;

            if (AdsManager.IsForcedAdEnabled()) AdsManager.HideBanner();

            PreviewImage.sprite = StoreController.GetSelectedProduct(CurrentProductType).PreviewSprite;

            instance.StartCoroutine(CircleScaleAnimation(AnimationConfig.CircleScale));
            instance.StartCoroutine(CircleColorAnimation(AnimationConfig.CircleColor));

            instance.StartCoroutine(PanelPositionAnim(AnimationConfig.PanelPosition));
            instance.StartCoroutine(PanelFadeAnim(AnimationConfig.PanelFade));

            instance.StartCoroutine(PreviewPositionAnim(AnimationConfig.PreviewPosition));
            instance.StartCoroutine(PreviewFadeAnim(AnimationConfig.PreviewFade));

            InitStoreUI(CurrentProductType);

            CoinsPanelTransform.localScale = Vector3.zero;
            CoinsPanelTransform.DOScale(0.5f, 0.2f).OnComplete(() => CoinsPanelTransform.DOScale(1, 0.3f).SetEasing(Ease.Type.BackOut));


            CloseButton.enabled = false;
            BackgroundCloseButton.enabled = false;

            CloseButtonTransform.localScale = Vector3.zero;
            CloseButtonTransform.DOScale(0.5f, 0.2f).OnComplete(() => CloseButtonTransform.DOScale(1, 0.3f).SetEasing(Ease.Type.BackOut).OnComplete(() => {
                CloseButton.enabled = true;
                BackgroundCloseButton.enabled = true;
            }));

            /*Tween.DelayedCall(0.1f, () => {
                LevelController.Environment.DisableTop();
            });

            Tween.DelayedCall(0.25f, () => {
                LevelController.Environment.DisableAll();
            });*/
        }

        private static IEnumerator CircleScaleAnimation(List<StoreAnimationConfig.AnimationStageScale> stages)
        {
            for(int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageScale scaleStage = stages[i];

                StoreBackground.transform.localScale = scaleStage.MinValue;

                yield return AnimationStageCoroutine(scaleStage, () => {
                    StoreBackground.transform.DOScale(scaleStage.MaxValue, scaleStage.Duration).SetEasing(scaleStage.Ease);
                });
            }
        }

        private static IEnumerator CircleColorAnimation(List<StoreAnimationConfig.AnimationStageColor> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageColor colorStage = stages[i];

                Color finalColor;

                if(colorStage.MinValue != Color.white)
                {
                    //StoreBackground.color = colorStage.MinValue;

                    finalColor = Color.white;
                } else
                {
                    StoreBackground.color = Color.white;

                    finalColor = StoreController.GetSelectedProduct(CurrentProductType).StoreBackgroundColor;
                }
                

                yield return AnimationStageCoroutine(colorStage, () => {
                    StoreBackground.DOColor(finalColor, colorStage.Duration).SetEasing(colorStage.Ease);
                });
            }
        }

        private static IEnumerator PanelPositionAnim(List<StoreAnimationConfig.AnimationStageAnchoredPosition> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageAnchoredPosition positionStage = stages[i];

                StorePanel.anchoredPosition = positionStage.MinValue;

                yield return AnimationStageCoroutine(positionStage, () => {
                    StorePanel.DOAnchoredPosition(positionStage.MaxValue, positionStage.Duration).SetEasing(positionStage.Ease);
                });
            }
        }

        private static IEnumerator PanelFadeAnim(List<StoreAnimationConfig.AnimationStageFade> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageFade fadeStage = stages[i];

                StorePanelCanvasGroup.alpha = fadeStage.MinValue;

                yield return AnimationStageCoroutine(fadeStage, () => {
                    StorePanelCanvasGroup.DOFade(fadeStage.MaxValue, fadeStage.Duration).SetEasing(fadeStage.Ease);
                });
            }
        }

        private static IEnumerator PreviewPositionAnim(List<StoreAnimationConfig.AnimationStageAnchoredPosition> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageAnchoredPosition positionStage = stages[i];

                ShineTransform.anchoredPosition = positionStage.MinValue;

                yield return AnimationStageCoroutine(positionStage, () => {
                    ShineTransform.DOAnchoredPosition(positionStage.MaxValue, positionStage.Duration).SetEasing(positionStage.Ease);
                });
            }
        }

        private static IEnumerator PreviewFadeAnim(List<StoreAnimationConfig.AnimationStageFade> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                StoreAnimationConfig.AnimationStageFade fadeStage = stages[i];

                //ShineCanvasGroup.alpha = fadeStage.MinValue;

                yield return AnimationStageCoroutine(fadeStage, () => {
                    PreviewImage.DOFade(fadeStage.MaxValue, fadeStage.Duration).SetEasing(fadeStage.Ease);
                });
            }
        }

        private static IEnumerator AnimationStageCoroutine(StoreAnimationConfig.AnimationStage stage, UnityAction animationEvent)
        {

            
            if (stage.Delay != 0) yield return new WaitForSeconds(stage.Delay);
            //Debug.Log(stage.Delay);
            animationEvent();
            yield return new WaitForSeconds(stage.Duration);
            
        }

        private static void InitStoreUI(StoreProductType type)
        {
            CurrentProductType = type;
            storeItemPool.ReturnToPoolEverything(true);
            pagesPool.ReturnToPoolEverything();
            pageDotsPool.ReturnToPoolEverything();
            pageDotsImagesList.Clear();
            isAnimationRunning = false;

            pagesAmount = pagesAmountForEachProductType[(int)type];
            UnlockRandomPriceText.text = StoreController.Database.GetProductPrice(CurrentProductType).ToString();
            CoinsForAdsText.text = StoreController.Database.CoinsForAdsAmount.ToString();

            productsByGroupDictionary = StoreController.Database.GetProductsByPageDictionary(CurrentProductType);
            currentPageIndex = (int)StoreController.GetProduct(StoreController.GetSelectedProductSkinID(CurrentProductType)).Page;
            pagesHolderLocalPosition = new Vector3(currentPageIndex * -Mathf.Clamp(Screen.width, canvasWidth, Screen.width), -320, 0);
            PagesHolderTransform.anchoredPosition = pagesHolderLocalPosition;

            for (int i = 0; i < pagesAmount; i++)
            {
                Transform page = pagesPool.GetPooledObject().GetComponent<Transform>();
                page.localPosition = page.localPosition.SetX(Mathf.Clamp(Screen.width, canvasWidth, Screen.width) * i);
                pagesList.Add(page.GetComponent<StorePage>());

                if (pagesAmount > 1)
                {
                    pageDotsLayout.enabled = true;
                    pageDotsImagesList.Add(pageDotsPool.GetPooledObject().GetComponent<Image>());
                    pageDotsImagesList[i].color = pageDotsImagesList[i].color.SetAlpha(currentPageIndex == i ? 1f : 0.4f);
                }

                InitPage(i);
            }

            UpdateProductPreview();
            UpdateCurrentPage(false);
            UpdateTabsState();

            Tween.DelayedCall(0.1f, () => pageDotsLayout.enabled = true);
        }

        private static void InitPage(int pageIndex)
        {
            pagesList[pageIndex].Init(productsByGroupDictionary[(StorePageName)pageIndex]);
        }

        private static void UpdateCurrentPage(bool redrawStorePage)
        {
            UpdatePagePoints();

            skinsOnCurrentPageList = productsByGroupDictionary[(StorePageName)currentPageIndex];

            closedSkinsOnCurrentPageIndexes.Clear();

            for (int i = 0; i < skinsOnCurrentPageList.Count; i++)
            {
                if (!skinsOnCurrentPageList[i].IsUnlocked() && skinsOnCurrentPageList[i].BehaviourType != BehaviourType.Dummy)
                {
                    closedSkinsOnCurrentPageIndexes.Add(i);
                }
            }

            UnlockRandomButtonObject.SetActive(closedSkinsOnCurrentPageIndexes.Count > 0);

            if (redrawStorePage)
            {
                pagesList[currentPageIndex].UpdatePage();
            }
        }

        private static void UpdatePagePoints()
        {
            for (int i = 0; i < pageDotsImagesList.Count; i++)
            {
                pageDotsImagesList[i].color = pageDotsImagesList[i].color.SetAlpha(currentPageIndex == i ? 1f : 0.4f);
            }
        }

        private IEnumerator RandomUnlockAnimation()
        {
            isAnimationRunning = true;
            float delay = 0.05f;
            int itemToUnlockIndex = closedSkinsOnCurrentPageIndexes[Random.Range(0, closedSkinsOnCurrentPageIndexes.Count)];
            List<StoreItemUI> storeItemsList = pagesList[currentPageIndex].StoreItemsList;

            if (closedSkinsOnCurrentPageIndexes.Count > 1)
            {
                for (int i = 0; i < storeItemsList.Count; i++)
                {
                    storeItemsList[i].SetHighlightState(false);
                }

                //Tween.DoFloat(0.05f, 0.5f, 3f, (float newValue) => delay = newValue);

                float duration = 3;
                float time = 0;

                float min = 0.05f;
                float max = 0.5f;

                while (delay < 0.5f)
                {
                    GameAudioController.PlayButtonAudio();
                    time += delay;

                    delay = min + (max - min) * (time / duration);

                    closedSkinsOnCurrentPageIndexes.Remove(itemToUnlockIndex);
                    int newIndex = closedSkinsOnCurrentPageIndexes[Random.Range(0, closedSkinsOnCurrentPageIndexes.Count)];
                    closedSkinsOnCurrentPageIndexes.Add(itemToUnlockIndex);
                    itemToUnlockIndex = newIndex;


                    for (int i = 0; i < storeItemsList.Count; i++)
                    {
                        storeItemsList[i].SetHighlightState(i == itemToUnlockIndex);
                    }

                    yield return new WaitForSeconds(delay);
                }

                yield return new WaitForSeconds(delay * 0.5f);
            }

            StoreProduct product = skinsOnCurrentPageList[itemToUnlockIndex];

            if (StoreController.TryToBuyProduct(product))
            {
                ChangePreview(product);
                UpdateCurrentPage(true);
            }

            isAnimationRunning = false;
        }

        public static void OnNewSkinSelected(StoreProduct product)
        {
            for (int i = 0; i < pagesList.Count; i++)
            {
                pagesList[i].UpdatePage();
            }

            UpdateProductPreview();
        }

        public static void OnTabPressed(StoreProductType productType)
        {
            if (CurrentProductType != productType)
            {
                InitStoreUI(productType);
                ChangePreview(StoreController.GetSelectedProduct(productType));
            }
        }

        private static void UpdateTabsState()
        {
            for (int i = 0; i < TabsList.Count; i++)
            {
                TabsList[i].SetActiveState(TabsList[i].Type == CurrentProductType);
            }
        }

        private static void UpdateProductPreview()
        {
            //PreviewImage.sprite = StoreController.GetSelectedProduct(CurrentProductType).PreviewSprite;

            //ClearOldPreviewModels();

            //Instantiate(StoreController.GetSelectedProduct(CurrentProductType).SkinPrefab, PreviewHolderTransform.position, Quaternion.Euler(0f, 140f, 0f), PreviewHolderTransform);
        }

        //private static void ClearOldPreviewModels()
        //{
        //    if (PreviewHolderTransform.childCount > 0)
        //    {
        //        for (int i = 0; i < PreviewHolderTransform.childCount; i++)
        //        {
        //            Destroy(PreviewHolderTransform.GetChild(i).gameObject);
        //        }
        //    }
        //}

        #region Swipe
        
        public void OnDrag(PointerEventData data)
        {
            float difference = data.pressPosition.x - data.position.x;

            pagesHolderTransform.anchoredPosition = Vector3.Lerp(pagesHolderLocalPosition, pagesHolderLocalPosition - new Vector3(difference, 0), 0.8f);
        }

        public void OnEndDrag(PointerEventData data)
        {
            float percentage = (data.pressPosition.x - data.position.x) / canvasWidth;
            float pageDeltaSign = Mathf.Sign(percentage);

            if (Mathf.Abs(percentage) >= swipeThereshold && ((pageDeltaSign < 0 && currentPageIndex > 0) || (pageDeltaSign > 0 && currentPageIndex < pagesAmount - 1)))
            {
                Vector3 newPosition = pagesHolderLocalPosition;

                currentPageIndex += (int)pageDeltaSign;
                UpdateCurrentPage(false);

                newPosition += Vector3.zero.SetX(-Mathf.Clamp(Screen.width, canvasWidth, Screen.width) * pageDeltaSign);
                pagesHolderTransform.DOAnchoredPosition(newPosition, 0.1f).OnComplete(() => pagesHolderLocalPosition = newPosition);
            }
            else
            {
                pagesHolderTransform.DOAnchoredPosition(pagesHolderLocalPosition, 0.1f);
            }
        }


        #endregion

        #region Buttons

        public void UnlockRandomButton()
        {
            GameAudioController.PlayButtonAudio();

            if (closedSkinsOnCurrentPageIndexes.Count <= 0)
                return;

            int price;

            if (CurrentProductType == StoreProductType.CharacterSkin)
            {
                price = StoreController.Database.CharacterSkinPrice;
            }
            else
            {
                price = StoreController.Database.EnvironmentSkinPrice;
            }

            if (GameController.CoinsCount < price) return;


            if (!isAnimationRunning)
            {
                GameController.SpendCoins(price);
                CoinsAmountText.text = GameController.CoinsCount.ToString();

                StartCoroutine(RandomUnlockAnimation());
            }
        }

        public void GetCoinsForAds()
        {
            GameAudioController.PlayButtonAudio();

            AdsManager.ShowRewardBasedVideo((bool haveReward) =>
             {
                 if (haveReward)
                 {
                     CurrencyCloudController.SpawnCurrency((RectTransform)GetCoinsButtonObject.transform, 15, "+" + StoreController.Database.CoinsForAdsAmount, delegate
                     {
                         Tween.DoFloat(GameController.CoinsCount, GameController.CoinsCount + StoreController.Database.CoinsForAdsAmount, 0.5f, (float value) =>
                         {
                             CoinsAmountText.text = Mathf.RoundToInt(value).ToString();
                         });

                         GameController.CollectCoins(StoreController.Database.CoinsForAdsAmount);
                     });
                 }
             });
        }

        public void CloseStoreButton()
        {

            if (!CanInteract) return;

            GameAudioController.PlayButtonAudio();

            /*StorePanel.DOAnchoredPosition(Vector3.down * 1600, 0.4f).SetEasing(Ease.Type.SineIn);
            ShineCanvasGroup.DOFade(0, 0.4f).OnComplete(() => {
                StoreBackground.transform.DOScale(Vector3.forward, 0.4f).SetEasing(Ease.Type.QuadIn);

                StoreBackground.DOColor(Color.white, 0.4f).SetEasing(Ease.Type.QuadIn).OnComplete(() =>
                {
                    StoreCanvas.enabled = false;
                });
            });*/


            instance.StartCoroutine(CircleScaleAnimation(AnimationConfig.CircleScaleBack));
            instance.StartCoroutine(CircleColorAnimation(AnimationConfig.CircleColorBack));

            instance.StartCoroutine(PanelPositionAnim(AnimationConfig.PanelPositionBack));
            instance.StartCoroutine(PanelFadeAnim(AnimationConfig.PanelFadeBack));

            instance.StartCoroutine(PreviewPositionAnim(AnimationConfig.PreviewPositionBack));
            instance.StartCoroutine(PreviewFadeAnim(AnimationConfig.PreviewFadeBack));

            CoinsPanelTransform.DOScale(0.5f, 0.3f).SetEasing(Ease.Type.BackIn).OnComplete(() => CoinsPanelTransform.DOScale(0, 0.2f));
            Tween.DelayedCall(AnimationConfig.BackDuration, () => {
                StoreCanvas.enabled = false;
                if(AdsManager.IsForcedAdEnabled()) AdsManager.ShowBanner();
            });

            CloseButton.enabled = false;
            BackgroundCloseButton.enabled = false;

            CloseButtonTransform.DOScale(0.5f, 0.3f).SetEasing(Ease.Type.BackIn).OnComplete(() => CloseButtonTransform.DOScale(0, 0.2f));

            /*Tween.DelayedCall(0.25f, () => {
                LevelController.Environment.EnableBottomLeftAndRight();
            });

            Tween.DelayedCall(0.4f, () => {
                LevelController.Environment.EnableTop();
            });*/

            //storeObjectsHolder.gameObject.SetActive(false);

        }

        #endregion

        #region Developement

        // currently unused because of Unity Editor bug: Screen.width returns different value if Game view is not active (click on this button in the ispector deactivates Game view which leads to wrong initialization of the store)
        //[Button("Open Store")]
        public void OpenStoreButtonDev()
        {
            OpenStore();
        }

        private void Update()
        {
            if (StoreCanvas.enabled && (Time.realtimeSinceStartup < 12))
            {
                AdsManager.HideBanner();
            }
        }

        #endregion
    }
}