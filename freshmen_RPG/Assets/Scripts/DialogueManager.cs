using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI guideText;
    public GameObject dialogueModal; // NPC 대사창
    public GameObject guideModal; // 안내창
    public GameObject stickerImg;
    public GameObject mapImg;
    public float typingSpeed = 0.05f;
    public float slideDuration = 0.5f;

    private string[] dialogues;
    private string[] guides;
    private int curr = 0;
    private bool isTyping = false;

    private Vector3 initialPosition; // 대사창 초기 위치
    private Vector3 targetPosition;  // 대사창 목표 위치

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

        guides = new string[]
        {
            "아이템을 얻기 위해 먼저 위와 같은 스티커를 찾아야 합니다.",
            "스티커는 학교 곳곳에 숨겨져 있으니 주의깊게 둘러보세요!",
            "그럼 가장 먼저 학생문화관으로 이동해볼까요? 지도를 따라 학문관을 찾아가봅시다!",
            "학문관 입구에서 스티커를 찾으면, 저를 다시 불러주세요! 그럼 즐거운 모험 되길 바라요~!"
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
                guideModal.SetActive(true);
                stickerImg.SetActive(true);
                curr = 0;
                PrintGuideModal();
            }
        }
        else if (guideModal.activeSelf)
        {
            curr++;

            if (curr == 2)
            {
                stickerImg.SetActive(false);
                mapImg.SetActive(true);
            }

            if (curr < guides.Length)
            {
                StartCoroutine(TypeDialogue(guides[curr], guideText));
            }
            else
            {
                SceneManager.LoadScene("Scenes/MainScene");
            }
        }
    }

    void PrintGuideModal()
    {
        guideText.text = "";
        StartCoroutine(TypeDialogue(guides[curr], guideText));
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

