using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private RectTransform menuPanel;

    private void OnEnable() {
        InputSystem.OnMenu += MenuButtonClick;
    }

    public void MenuButtonClick(bool _state) {
        PauseManager.OnPause?.Invoke(_state);
        menuPanel.gameObject.SetActive(_state);
    }

    private void OnDisable() {
        InputSystem.OnMenu -= MenuButtonClick;
    }
}
