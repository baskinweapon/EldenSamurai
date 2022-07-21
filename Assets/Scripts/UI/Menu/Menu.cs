using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private RectTransform menuPanel;

    private void OnEnable() {
        PauseManager.OnPause += Pause;
    }

    public void MenuButtonClick(bool _state) {
        InputSystem.instance.MenuPressed();
    }

    public void Pause(bool _state) {
        menuPanel.gameObject.SetActive(_state);
    }

    private void OnDisable() {
        PauseManager.OnPause -= Pause;
    }
}
