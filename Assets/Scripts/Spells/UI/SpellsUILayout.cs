using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsUILayout : MonoBehaviour {
    public GameObject objectPrefab; // префаб объекта для расстановки
    public int fibonacciLimit = 20; // количество элементов ряда Фибоначчи для отображения
    public float objectDistance = 100f; // расстояние между объектами

    public Sprite[] sprite;
    
    private void Start() {
        int[] fibonacciNumbers = GenerateFibonacciNumbers(fibonacciLimit);
        for (int i = 0; i < fibonacciNumbers.Length; i++)
        {
            int number = fibonacciNumbers[i];
            float y = i;
            for (int j = 0; j < number; j++) {
                float x = j - number / 2;
                GameObject obj = Instantiate(objectPrefab, transform);
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                var o = obj.GetComponent<SpellCell>();
                o.Set(sprite[Random.Range(0, sprite.Length - 1)], "Default");
                var _x = j == number - 1 ? -number / 2 : j + 1 - number / 2;
                var vec = new Vector2((j + 1 - number / 2) * objectDistance, y * objectDistance);
                o.DrawLine(vec);
                rectTransform.localPosition = new Vector2(x * objectDistance, y * objectDistance);
            }
        }
    }

    // генерация первых n элементов ряда Фибоначчи
    private int[] GenerateFibonacciNumbers(int n) {
        int[] fibonacciNumbers = new int[n];
        for (int i = 0; i < n; i++) {
            if (i == 0 || i == 1) {
                fibonacciNumbers[i] = 1;
            }
            else {
                fibonacciNumbers[i] = fibonacciNumbers[i - 1] + fibonacciNumbers[i - 2];
            }
        }
        return fibonacciNumbers;
    }
    
}
