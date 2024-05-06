#pragma warning disable 649

using UnityEngine;
using Watermelon;

public class EnvironmentSkinProduct : StoreProduct
{
    [Space]
    [SerializeField] GameObject skinPrefab;
    public GameObject SkinPrefab => skinPrefab;

    [SerializeField] GameObject[] obstaclePrefabs;

    public EnvironmentSkinProduct()
    {
        BehaviourType = BehaviourType.Default;
        Type = StoreProductType.EnvironmentSkin;
    }

    public override void Init()
    {

    }

    public override void Unlock()
    {
        Tween.NextFrame(delegate 
        {
            GameController.SetEnvironmentSkin(this);
        });
    }

    public override bool IsUnlocked()
    {
        return StoreController.IsProductUnlocked(ID);
    }

    public override bool CanBeUnlocked()
    {
       // Debug.Log("[Store Module] Add unlock condition check here.");
        return true;
    }

    public GameObject GetObstacleObject(int index)
    {
        if (obstaclePrefabs.IsInRange(index))
            return obstaclePrefabs[index];

        return null;
    }
}