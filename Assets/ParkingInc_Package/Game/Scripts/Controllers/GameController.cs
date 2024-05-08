#pragma warning disable 649

using UnityEngine;
using Watermelon;
using UnityEngine.EventSystems;
using System;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    [SerializeField] LevelDatabase levelDatabase;
    [SerializeField] GameConfigurations gameConfigurations;

    public static GameConfigurations GameConfigurations => instance.gameConfigurations;

    public static LevelDatabase LevelDatabase => instance.levelDatabase;

    public static int CurrentLevelId 
    { 
        get => PrefsSettings.GetInt(PrefsSettings.Key.CurrentLevelID);
        private set => PrefsSettings.SetInt(PrefsSettings.Key.CurrentLevelID, value);
    }

    public static int ActualLevelId
    {
        get => PrefsSettings.GetInt(PrefsSettings.Key.ActualLevelID);
        private set => PrefsSettings.SetInt(PrefsSettings.Key.ActualLevelID, value);
    }

    public static int CarsSkinId
    {
        get => PrefsSettings.GetInt(PrefsSettings.Key.CarSkinID);
        private set => PrefsSettings.SetInt(PrefsSettings.Key.CarSkinID, value);
    }

    public static int EnvironmentSkinId
    {
        get => PrefsSettings.GetInt(PrefsSettings.Key.EnvironmentSkinID);
        private set => PrefsSettings.SetInt(PrefsSettings.Key.EnvironmentSkinID, value);
    }

    public static int CoinsCount
    {
        get => PrefsSettings.GetInt(PrefsSettings.Key.CoinsCountID);
        private set => PrefsSettings.SetInt(PrefsSettings.Key.CoinsCountID, value);
    }

    private static CarSkinProduct characterSkinProduct;
    public static CarSkinProduct CharacterSkinProduct => characterSkinProduct;

    private static EnvironmentSkinProduct environmentSkinProduct;
    public static EnvironmentSkinProduct EnvironmentSkinProduct => environmentSkinProduct;

    public static bool StartStage { get; private set; }
    public static bool WinStage { get; private set; }

    public static int TurnsAfterRewardVideo { get; set; }

    private void OnEnable()
    {
      
        StoreController.OnProductSelected += OnProductSelected;
    }
    
    private void OnDisable()
    {
        
        StoreController.OnProductSelected -= OnProductSelected;
    }

    private bool ExtraInterstitialCondition()
    {
        if(TurnsAfterRewardVideo <= 4)
        {
            Debug.Log("[AdsManager]: Custom condition - TurnsAfterRewardVideo <= 4");

            return false;
        }

        return true;
    }

    private void Start()
    {
        instance = this;

        TurnsAfterRewardVideo = 5;

        CarSkinProduct characterSkinProduct = StoreController.GetSelectedProduct(StoreProductType.CharacterSkin) as CarSkinProduct;
        GameController.characterSkinProduct = characterSkinProduct;

        EnvironmentSkinProduct environmentSkinProduct = StoreController.GetSelectedProduct(StoreProductType.EnvironmentSkin) as EnvironmentSkinProduct;
        GameController.environmentSkinProduct = environmentSkinProduct;

        LevelController.Init();

        StartLevel(ActualLevelId, true);

        StartStage = true;
        WinStage = false;

        QualitySettings.vSyncCount = 1;
    }

    public static void StartGame()
    {
        StartStage = false;
        LevelController.LoadObstaclesAndCars();
        UIController.SetSkipButtonVisibility(true);
        CameraController.ChangeAngleToGamePosition(LevelController.CurrentLevel);
    }

    public static void NextLevel(bool withTransition = true)
    {
        CalculateNextLevel(withTransition);
        GameLoose.intance.RestartTimer();

        /*AdsManager.ShowInterstitial((isDisplayed) =>
        {
            if (isDisplayed)
            {
                CalculateNextLevel(withTransition);
            }
            else
            {
                CalculateNextLevel(withTransition);
            }
        });*/
    }

    private static void CalculateNextLevel(bool withTransition)
    {
        CurrentLevelId++;
        if (CurrentLevelId >= LevelDatabase.LevelsCount)
        {
            int oldLevel = ActualLevelId;
            do
            {
                ActualLevelId = UnityEngine.Random.Range(0, LevelDatabase.LevelsCount);
            } while (ActualLevelId == oldLevel);
        }
        else
        {
            ActualLevelId = CurrentLevelId;
        }

        UITouchHandler.CanReplay = false;

        if (withTransition)
        {
            StartLevelWithTransition(ActualLevelId);
        }
        else
        {
            StartLevel(ActualLevelId, StartStage);
        }
    }

    private static void PreviousLevel()
    {
        if(ActualLevelId > 0)
        {
            ActualLevelId--;
            CurrentLevelId = ActualLevelId;
        }

        StartLevel(ActualLevelId, StartStage);
    }

    private static void FirstLevel()
    {
        ActualLevelId = 0;
        CurrentLevelId = 0;

        StartLevel(ActualLevelId, StartStage);
    }

    public static void PrevLevelDev()
    {
        LevelController.DestroyLevel();
        PreviousLevel();
    }

    public static void FirstLevelDev()
    {
        LevelController.DestroyLevel();
        FirstLevel();
    }

    public static void StartLevel(int levelId, bool withLogo = false)
    {
        Level level = LevelDatabase.GetLevel(levelId);

        if(withLogo) {
            LevelController.InitLevelWithLogo(level);
        } else
        {
            LevelController.InitLevel(level);
        }
        
        CameraController.Init(level);
        UIController.SetLevel(CurrentLevelId);
    }

    public static void StartLevelWithTransition(int levelId) 
    {
        Level level = LevelDatabase.GetLevel(levelId);
        LevelController.InitLevelWithTransition(level);

        UIController.SetLevel(CurrentLevelId);

        WinStage = false;
    }

    public static void FinishLevel()
    {
        WinStage = true;

        GameAudioController.VibrateLevelFinish();
    }

    public static void SkipLevel()
    {
        instance.StartCoroutine(LevelObjectsSpawner.HideBounceCars());
        LevelController.FinishLevel();
        FinishLevel();
    }

    public static void ReplayLevel()
    {
        if(!LevelController.isReplaying) LevelController.ReplayLevel();
    }

    public static void CollectCoins(int amount)
    {
        CoinsCount += amount;
    }

    public static void SpendCoins(int amount)
    {
        CoinsCount -= amount;
    }

    public static void SetCarSkin(CarSkinProduct characterSkinProduct)
    {
        GameController.characterSkinProduct = characterSkinProduct;

        CarsSkinId = characterSkinProduct.ID;

        // Remove cars
        LevelPoolHandler.DeleteMovablesPools();
        LevelPoolHandler.InitMovablePools();

        if (!GameController.StartStage && !LevelObjectsSpawner.IsMovablesEmpty)
        {
            LevelObjectsSpawner.SpawnCars();
        }
    }

    public static void SetEnvironmentSkin(EnvironmentSkinProduct environmentSkinProduct)
    {
        GameController.environmentSkinProduct = environmentSkinProduct;

        EnvironmentSkinId = environmentSkinProduct.ID;

        LevelController.ResetEnvironment();

        // Remove cars
        LevelPoolHandler.DeleteObstaclePools();
        LevelPoolHandler.InitObstaclePools();

        if (!GameController.StartStage && !LevelObjectsSpawner.IsMovablesEmpty)
        {
            LevelObjectsSpawner.SpawnObstacles();
        }
    }

    private void OnProductSelected(StoreProduct product)
    {
        if(product.Type == StoreProductType.CharacterSkin)
        {
            CarSkinProduct carSkinProduct = product as CarSkinProduct;
            if(carSkinProduct != null)
            {
                SetCarSkin(carSkinProduct);
            }
        }
        else if(product.Type == StoreProductType.EnvironmentSkin)
        {
            EnvironmentSkinProduct environmentSkinProduct = product as EnvironmentSkinProduct;
            if (environmentSkinProduct != null)
            {
                SetEnvironmentSkin(environmentSkinProduct);
            }
        }
    }
}
