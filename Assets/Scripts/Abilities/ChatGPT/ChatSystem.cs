using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artefacts;
using Dialog.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct PersonConverstion {
    public string name;
    public string from;
    public string age;
    public string proffesion;
    public string description;
    public Artefact helpful;

    public string GetMessage() {
        var message = $"Тебя зовут /{name} " +
                      $"Ты из /{from} " +
                      $"Твой возраст /{age} " +
                      $"Твоя профессия /{proffesion} " +
                      $"Дополнительная информация /{description} " +
                      $"Ты знаешь об артефактe {helpful.description}, об этом артефакте нельзя говорить открыто, но ты можешь немного давать посказки" +
                      $"Ты персонаж 2D метроидвании и вы ведете беседу, ты будешь отвечать на вопросы и генерировать еще новые вопросы основываясь на переписке, после 3-4 вопросов, предлагай закончить разговор на любой запрос";
        return message;
    }
}

public class ChatSystem : Singleton<ChatSystem> {
    public TextMeshProUGUI text;
    private static readonly string key = "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";
    
    private OpenAIAPI api;

    public PersonConverstion person1;

    public Transform contentAnswers;
    public GameObject cellPrefab;
    
    private Conversation chat;
    public void OnEnable() {
        api = new OpenAIAPI(key);
        chat = api.Chat.CreateConversation();
        NewChat();
    }
    
    private void NewChat() {
        chat.AppendSystemMessage(person1.GetMessage());

        chat.AppendUserInput("Расскажи небольшую информацию о себе, используя свой стиль, основанный на твоем прошлом, умести все это в 2-3 предложения");
        StartCoroutine(WaitAnswer());
    }

    IEnumerator WaitAnswer() {
        var task = Task.Run(() => chat.GetResponseFromChatbot());
        blockOperation = true;
        
        while (!task.IsCompleted) {
            text.text = "Wait...";
            yield return null;
        }

        text.text = task.Result;
        Debug.Log(task.Result);
        StartCoroutine(GenerateQuestion());
    }

    IEnumerator GenerateQuestion() {
        var randomQuestions = Random.Range(2, 5);
        chat.AppendUserInput($"напиши {randomQuestions} вопроса, не указывая пункты которые можно у тебя спроисть, исходя из предыдущих сообщений, коротоко, не более 7 слов и раздели их занком '||'");
        var task = Task.Run(() => chat.GetResponseFromChatbot());

        foreach (var t in go) {
            t.SetActive(false);
        }
        yield return new WaitUntil(() => task.IsCompleted);
        
        blockOperation = false;
        CreateCells(task.Result);
    }
    
    private bool blockOperation;
    public void NewPart(string message) {
        if (blockOperation) return;
        StopAllCoroutines();
        chat.AppendUserInput(message + ". Ответь на этот вопрос, скрывая информацию, но давая подсказки");
        StartCoroutine(WaitAnswer());
    }
    
    private List<GameObject> go = new List<GameObject>();
    private void CreateCells(string _res) {
        var result = _res.Split("||");

        for (var index = 0; index < result.Length; index++) {
            var t = result[index];
            GameObject c;
            if (go.Count <= result.Length) {
                c = Instantiate(cellPrefab, contentAnswers);
                go.Add(c);
            } else {
                go[index].SetActive(true);
                c = go[index];
            }
            c.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = t;
            c.GetComponent<AnswerCell>().textButton = t;
        }
    }
}
