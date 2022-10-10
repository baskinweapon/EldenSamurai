using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour {
    public TextMeshProUGUI tmp;
    public Texture2D mainTexture;
    public RectTransform rect;

    [Range(0, 100)]
    public float speed;
    
    private Material mat;
    private static readonly int _HealthStr = Shader.PropertyToID("_Health");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    protected float currentValuePercent;
    protected float maxValue;
    protected float persistantValue;
    protected virtual void SetValues() {
    }
    
    private void Start() {
        mat = rect.GetComponent<Image>().material;
        mat.SetTexture(MainTex, mainTexture);
        
        SetValues();
    }

    private float sliderValue;
    private void Update() {
        SetValues();
        var end = currentValuePercent;
        var dist = end - sliderValue;
        if (Mathf.Abs(dist) >= 0.001f) {
            sliderValue = Mathf.Lerp(mat.GetFloat(_HealthStr), end, Time.deltaTime * speed);
            
            if (dist <= 0.01f) sliderValue = end;
            
            mat.SetFloat(_HealthStr, sliderValue);
            SetText(sliderValue * maxValue);
        }
    }

    private void SetText(float value) {
        tmp.text = (int)value + " / " + maxValue;
    }
}
