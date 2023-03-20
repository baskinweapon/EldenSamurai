using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCell : MonoBehaviour {
    public TextMeshProUGUI text;
    public Image image;
    public UILineRenderer lineRenderer;
    
    
    public void Set(Sprite _img, string _name) {
        text.text = _name;
        image.sprite = _img;
    }

    public void DrawLine(Vector2 endPoint) {
        lineRenderer = gameObject.AddComponent<UILineRenderer>();
        lineRenderer.Points = new Vector2[20];
        lineRenderer.color = Color.black;
        for (int i = 0; i < 20; i++) {
            lineRenderer.Points[i].x = Mathf.Lerp(transform.position.x, endPoint.x, i / 20);
            lineRenderer.Points[i].y = Mathf.Lerp(transform.position.y, endPoint.y, i / 20);
        }
    }
    
    
}
