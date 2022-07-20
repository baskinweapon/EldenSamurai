using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private RectTransform menuPanel;
    
    public void MenuButtonClick(bool _state) {
        menuPanel.gameObject.SetActive(_state);
    }
    
}
