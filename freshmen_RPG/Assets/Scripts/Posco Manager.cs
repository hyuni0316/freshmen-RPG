using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PoscoManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI guideText;
    public GameObject dialogueModal; // NPC 대사창
    public GameObject guideModal; // 안내창
    public GameObject stickerImg;
    public GameObject mainImg;
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
            "와! 포스코관도 잘 찾아오셨군요! 역시 대단해요!",
        "포스코관은 줄여서 포관이라고 많이 부르고, 주로 대형 교양강의들이 이루어지는 곳이에요.",
        "신입생을 뽑기 위한 학부 입학, 대학원 입학 수시 면접도 주로 여기에서 시행하는데, 시행하는 날에는 건물 출입을 막지만 시행하기 전에는 사전 공지를 꼭 하니 걱정하실 필요는 없어요!",
        "또한 지하 1층에 오봉도시락과 더벤티가 있어서, 배고프실 때 찾아가시기 좋아요!",
        "다만 지하 1층도 지상과 연결되어 지하 1층과 1층을 혼동하시는 분들이 많아서, 꼭 현재 층수를 잘 확인하고 이동하시는 게 좋아요!",
        "종합과학관과 연결되어 있어서 공대로 이동할 때 편리한데, 종합과학관으로 갈 수 있는 쪽문은 4층에 있답니다!",
        };

        guides = new string[]
        {
           "앗! 여기 4층 쪽문 앞에 몬스터가 된 학생이 등장했대요! 포관에서 밤새 시험 공부를 하다가 그대로 로봇이 된 몬스터라던데요..",
           "4층 쪽문 앞에 있는 리무버블 스티커를 인식하면 몬스터와 전투할 수 있어요~ ",
           "꼭 시험으로 인해 흑화한 학생을 구해주세요! 건투를 빌어요!"
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
                mainImg.SetActive(false);
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

