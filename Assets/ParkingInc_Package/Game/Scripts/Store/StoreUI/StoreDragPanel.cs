using UnityEngine;
using UnityEngine.EventSystems;

namespace Watermelon
{
    public class StoreDragPanel : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] StoreUIController storeController;

        public void OnDrag(PointerEventData data)
        {
            storeController.OnDrag(data);
        }

        public void OnEndDrag(PointerEventData data)
        {
            storeController.OnEndDrag(data);
        }
    }
}
