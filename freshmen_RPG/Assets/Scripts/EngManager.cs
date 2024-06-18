using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EngManager : MonoBehaviour
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
            "공학관에 무사히 도착하셨군요! 이 언덕을 기어코 올라오시다니 정말 멋져요…!!!",
           "공학관은 아산공학관과 신공학관으로 이루어져 있는 건물이에요.",
           "수강신청을 할 때, 아산공학관은 공학A, 신공학관은 공학B라고 써있으니 기억해두시면 좋을 거에요!",
           "아산공학관 지하 1층에는 공대강당이 있어요. 내려가는 계단은 딱 한가지인데, 생협 옆에 있으니 참고하세요!",
           "배고플 때는 신공학관 지하 2층에는 학생식당이 있어서 점심 식사가 가능해요!",
           "신공학관 2층에는 공대도서관이 있는데, 공대생들의 요충지에요. ECC와 중앙도서관이 먼 공대생들에게 공강 시간과 시험 기간에 열심히 공부할 수 있는 유일한 장소랍니다!"           
        };

        guides = new string[]
        {
           "앗! 여기 공대도서관에 몬스터가 된 학생이 등장했대요!",
           "사계절 계속되는 공대의 추위와 성적에 얼어붙은 몬스터라고 하네요..",
           "공대도서관 앞에 있는 리무버블 스티커를 인식하면 몬스터와 전투할 수 있어요.",
           "꼭 성적으로 인해 얼어붙은 학생을 구해주세요! 건투를 빌어요!"
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

