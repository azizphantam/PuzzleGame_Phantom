using UnityEngine;
using Watermelon;

public class CarSkinProduct : StoreProduct
{
    [Space]
    [SerializeField] GameObject[] movableGameObjects;

    public CarSkinProduct()
    {
        BehaviourType = BehaviourType.Default;
        Type = StoreProductType.CharacterSkin;
    }

    public override void Init()
    {

    }

    public override void Unlock()
    {
        GameController.SetCarSkin(this);
    }

    public override bool IsUnlocked()
    {
        return StoreController.IsProductUnlocked(ID);
    }

    public override bool CanBeUnlocked()
    {
        //Debug.Log("[Store Module] Add unlock condition check here.");
        return true;
    }

    public GameObject GetMovableObject(int index)
    {
        if (movableGameObjects.IsInRange(index))
            return movableGameObjects[index];

        return null;
    }
}