using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EpilogueManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;    
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueModal; // NPC 대사창
    public float typingSpeed = 0.05f;
    public float slideDuration = 0.5f;

    public GameObject video;
    public GameObject endingImg;

    private string[] dialogues;
    private int curr = 0;
    private bool isTyping = false;

    private Vector3 initialPosition; // 대사창 초기 위치
    private Vector3 targetPosition;  // 대사창 목표 위치

    void Start()
    {
        // 일단 이름 바로 넣긴 했는데 시작화면에 플레이어 이름 넣는 인풋필드 만들까?
        dialogues = new string[]
        {
            "정말 대단해요, 새로니 벗!",
            "덕분에 몬스터가 된 학생들이 모두 원래대로 돌아왔어요.",
            "이제 이화여자대학교의 평화가 돌아왔어요!",
            "새로니 벗, 앞으로도 이화여자대학교에서 멋진 추억을 많이 만들기를 바라요.",
            "그럼, 안녕"
        };


        initialPosition = dialogueModal.transform.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y - Screen.height, initialPosition.z);

        StartCoroutine(TypeDialogue(dialogues[curr], dialogueText));
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
        if (dialogueModal.activeSelf)
        {
            curr++;

            if (curr < dialogues.Length)
            {
                StartCoroutine(TypeDialogue(dialogues[curr], dialogueText));
            }
            else
            {
                StartCoroutine(SlideOutDialogue());
                titleText.text = "The End";
                video.SetActive(false);
                endingImg.SetActive(true);
                curr = 0;
            }
        }
    }

    IEnumerator TypeDialogue(string dialogue, TextMeshProUGUI txt)
    {
        isTyping = true;
        txt.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            txt.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    IEnumerator SlideOutDialogue()
    {
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            dialogueModal.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dialogueModal.SetActive(false); // 대사창 비활
        dialogueModal.transform.position = initialPosition;
    }
}

