using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanelTimer : MonoBehaviour {
    [SerializeField] 
    private GameObject timerObject;
    
    [SerializeField]
    private Image circleFill;

    [SerializeField]
    private TextMeshProUGUI timeText;

    public float speed;
    public float spellCountDown = 2;

    private void Start() {
        timerObject.SetActive(false);
    }

    public void OnButtonPress() {
        timerObject.SetActive(true);
        circleFill.fillAmount = 1f;
        StopAllCoroutines();
        StartCoroutine(Timer());
    }
    
    

    // 2sec fill = 1 za 2 sec do 0
    IEnumerator Timer() {
        var countdown = spellCountDown;
        while (countdown >= 0) {
            timeText.text = TimeSpan.FromSeconds(countdown).ToString(@"ss\:ff");
            countdown -= Time.deltaTime;
            circleFill.fillAmount -= Time.deltaTime / spellCountDown;
            yield return null;
        }
        
        circleFill.fillAmount = 1f;
        timerObject.SetActive(false);
    }
    
}
