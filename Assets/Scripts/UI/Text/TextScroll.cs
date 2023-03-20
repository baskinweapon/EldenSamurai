using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    public float scrollSpeed = 50.0f;
    public bool pingPong = true;
    public TextMeshProUGUI scrollingText;

    private Vector3 originalPosition;
    private bool isScrolling = true;
    private bool isGoingUp = true;

    public void ReCalculate() {
        StopAllCoroutines();
        scrollingText.rectTransform.localPosition = originalPosition;
        StartCoroutine(ScrollText());
    }
    
    void Start() {
        scrollingText = GetComponent<TextMeshProUGUI>();
        originalPosition = scrollingText.rectTransform.localPosition;
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText()
    {
        while (isScrolling)
        {
            if (pingPong)
            {
                if (isGoingUp)
                {
                    scrollingText.rectTransform.localPosition += Vector3.up * (scrollSpeed * Time.deltaTime);
                    if (scrollingText.rectTransform.localPosition.y > originalPosition.y + scrollingText.preferredHeight)
                    {
                        isGoingUp = false;
                    }
                }
                else
                {
                    scrollingText.rectTransform.localPosition -= Vector3.up * (scrollSpeed * Time.deltaTime); 
                    if (scrollingText.rectTransform.localPosition.y < originalPosition.y)
                    {
                        isGoingUp = true;
                    }
                }
            }
            else
            {
                scrollingText.rectTransform.localPosition += Vector3.up * (scrollSpeed * Time.deltaTime);
                if (scrollingText.rectTransform.localPosition.y < originalPosition.y - scrollingText.preferredHeight)
                {
                    scrollingText.rectTransform.localPosition = originalPosition;
                }
            }
            yield return null;
        }
    }
}
