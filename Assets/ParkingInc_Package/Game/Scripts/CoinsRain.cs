using UnityEngine;
using Watermelon;

public class CoinsRain : MonoBehaviour
{
    private static readonly string COINS_POOL_NAME = "coins";

    private static Pool coinsPool;

    private void Awake()
    {
        coinsPool = PoolManager.GetPoolByName(COINS_POOL_NAME);
    }

    public static void RainCoins(int amount)
    {
        float altitude = CameraController.Position.y - 10;

        for(int i = 0; i < amount; i++)
        {
            Coin coin = coinsPool.GetPooledObject().GetComponent<Coin>();
            coin.Init(
                initialVelocity: Vector3.down * (20 + Random.value * 20),
                position: GetRandomRoadPosition((Road)(i % 4), altitude),
                rotation: new Quaternion(Random.value, Random.value, Random.value, Random.value)
                );

        }
    }

    private static Vector3 GetRandomRoadPosition(Road road, float altitude)
    {
        float levelSizeX = LevelController.CurrentLevel.Size.x;
        float levelSizeY = LevelController.CurrentLevel.Size.y;

        switch (road)
        {
            case Road.Left:
                return new Vector3(Random.Range(-3f, 2), altitude + Random.value * 20, Random.Range(-3f, levelSizeY + 3));
            case Road.Top:
                return new Vector3(Random.Range(-3f, levelSizeX + 3), altitude + Random.value * 20, Random.Range(levelSizeY - 2, levelSizeY + 3));
            case Road.Right:
                return new Vector3(Random.Range(levelSizeX - 2, levelSizeX + 3), altitude + Random.value * 20, Random.Range(-3f, levelSizeY + 3));
            case Road.Bottom:
                return new Vector3(Random.Range(-3f, levelSizeX + 3), altitude + Random.value * 20, Random.Range(-3f, 2));

        }

        return Vector3.zero;
    }

    private enum Road
    {
        Left = 0, 
        Top = 1, 
        Right = 2, 
        Bottom = 3
    }
}
