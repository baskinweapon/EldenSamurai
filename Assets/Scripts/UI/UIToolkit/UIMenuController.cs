using UnityEngine;
using UnityEngine.UIElements;

namespace UI.UIToolkit {
    public class UIMenuController : MonoBehaviour {
        private UIDocument doc;
        
        private Button menuButton;
        private VisualElement elem;
        
        private void Awake() {
            doc = GetComponent<UIDocument>();

            menuButton = doc.rootVisualElement.Q<Button>("OpenMenuButton");
            elem = doc.rootVisualElement.Q<VisualElement>("MenuPanel");
            menuButton.clicked += MenuMenuButtonClick;
        }

        private void MenuMenuButtonClick() {
            Debug.Log("Press menu button");
            elem.visible = !elem.visible;
        }
    }
}