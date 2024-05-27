using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private string[] dialogues;
    private int curr = 0;
    private bool isTyping = false;

    void Start()
    {
        // 일단 이름 바로 넣긴 했는데 시작화면에 플레이어 이름 넣는 인풋필드 만들까?
        dialogues = new string[]
        {
            "안녕하세요, 가빈 벗! 드디어 꿈에 그리던 이화여자대학교에 입학한 것을 진심으로 축하해요!",
            "하지만 조심해야해요! 요즘 극심한 시험 스트레스로 이화여대 학생들이 몬스터가 되어 사람들을 공격한다는 흉흉한 소문이 돌고 있어요...",
            "몬스터가 된 학생들을 구하기 위해 가빈 벗의 도움이 절실해요!",
            "그러나 최종 몬스터 보스와 싸우기 위해서는 능력을 올려야해요! 능력은 아이템을 사용해 올릴 수 있답니다.",
            "우선 아이템을 모으기 위해 학교를 둘러보도록 하세요!"
        };

        StartCoroutine(TypeDialogue(dialogues[curr]));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            OnScreenClick();
        }
    }

    public void OnScreenClick()
    {
        curr++;

        if (curr < dialogues.Length)
        {
            StartCoroutine(TypeDialogue(dialogues[curr]));
        }
        else
        {
            curr = dialogues.Length - 1;
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}

public class ClickableArea : MonoBehaviour
{
    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            GameObject.FindObjectOfType<DialogueManager>().OnScreenClick();
        }
    }
}

