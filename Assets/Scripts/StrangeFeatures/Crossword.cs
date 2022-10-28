using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct Word {
    public int id;
    public string word;
    public string secondWord;
    public int meargeInt;
    public int meatgeSecondInt;
    public int mergeCountSize;
}

namespace StrangeFeatures {
    public class Crossword : MonoBehaviour {
        public GameObject prefab;
        public Transform parent;
        public string[] words;

        public List<Word> resultWords = new List<Word>();

        public void Start() {
            CreatePuzzle();
        }

        private bool Horizontal;
        private void CreatePuzzle() {
            for (int i = 0; i < words.Length - 1; i++) {
                var word1 = words[i];
                var word2 = words[i + 1];
                
                FindSimiliarChar(word1, word2);
            }

            var pos = new Vector2(0, 0);
            foreach (var word in resultWords) {
                for (int i = 0; i < word.word.Length; i++) {
                    var obj = Instantiate(prefab, parent);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = word.word[i].ToString();
                    Vector2 vec;
                    if (!Horizontal) {
                        vec = new Vector2(50, 0);
                    } else {
                        vec = new Vector2(0, -50);
                    }
                    pos += vec;
                    obj.GetComponent<RectTransform>().position += (Vector3)pos;
                }
                
                if (!Horizontal) {
                    pos = new Vector2((word.meargeInt + 1) * 50, (word.meatgeSecondInt + 1) * 50);
                } else {
                    pos = new Vector2((word.meatgeSecondInt + 1)  * 50, (word.meargeInt + 1) * 50);
                }
                Horizontal = !Horizontal;
                
                
                //debug
                Debug.Log("First word = " + word.word);
                Debug.Log("Second word = " + word.secondWord);
                Debug.Log("merge = " + word.meargeInt);
            }
        }

        private void FindSimiliarChar(string word1, string word2) {
            for (int j = 0; j < word1.Length; j++) {
                for (int i = 0; i < word2.Length; i++) {
                    if (word1[j].Equals(word2[i])) {
                        var w = new Word {
                            id = 0,
                            word = word1,
                            secondWord = word2,
                            meargeInt = j,
                            meatgeSecondInt = i,
                            mergeCountSize = 1,
                        };
                        resultWords.Add(w);
                        return;
                    }
                }
            }
        }
    }
}