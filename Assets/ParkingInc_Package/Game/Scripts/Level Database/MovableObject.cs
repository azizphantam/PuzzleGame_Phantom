#pragma warning disable 649

using UnityEngine;
using Watermelon;

[CreateAssetMenu(menuName = "Level Database/Movable Object", fileName = "Movable Object")]
public class MovableObject: ScriptableObject
{
    [SerializeField] int shopIndex;
    [SerializeField] private FieldElement fieldElement;

    public GameObject Prefab
    {
        get
        {
            CarSkinProduct carSkinProduct = StoreController.GetSelectedProduct(StoreProductType.CharacterSkin) as CarSkinProduct;

            return carSkinProduct.GetMovableObject(shopIndex);
        }
    }
    public Vector2Int Size => fieldElement.Size;
}
