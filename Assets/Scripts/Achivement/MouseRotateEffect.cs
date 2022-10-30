using UnityEngine;
using UnityEngine.EventSystems;

namespace Achivement {
    public class MouseRotateEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private RectTransform tr;
        
        public float clampValue = 35f;
        public float speed = 2f;

        private bool isCasting;
        private void Update() {
            var bounds = (Vector2)tr.position;
            var point = InputSystem.instance.GetMousePosition();
            var scrPoint = bounds;
            var offset = point - scrPoint;
            if (!isCasting) {
                transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.identity, Time.deltaTime * speed);
                return;
            }
            
            offset.x = Mathf.Clamp(offset.x, -clampValue, clampValue);
            offset.y = Mathf.Clamp(offset.y, -clampValue, clampValue);
            var rotation = Quaternion.Euler(-offset.y, offset.x, 0f);
            transform.rotation = Quaternion.Lerp( transform.rotation, rotation, Time.deltaTime * speed);
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            isCasting = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isCasting = false;
        }
    }
}