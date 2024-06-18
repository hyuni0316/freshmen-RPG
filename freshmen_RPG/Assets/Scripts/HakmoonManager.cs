using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HakmoonManager : MonoBehaviour
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
            "와! 무사히 학생문화관에 찾아오셨군요! 다행이에요!",
           "학생문화관은 줄여서 학문관이라고 많이 부르고, 다양한 동아리방이 밀집해 있는 건물이에요.",
           "스포츠 GX로 요가, 방송댄스, 줌바댄스, 필라테스, 스쿼시 등을 학문관에서 수강할 수 있어요! 회원카드가 필요하니 유레카에서 신청하시면 된답니다~",
           "학문관에는 지하 2층에 생협과 이마트 24가 있어서, 배고프실 때 언제든 찾아가실 수도 있어요~",
           "또 다른 주요 건물인 포스코관과 연결되어 있어서 편리한데, 포스코관으로 갈 수 있는 쪽문은 3층에 있답니다!",
           
        };

        guides = new string[]
        {
           "앗! 여기 지하 2층 생협 앞에 몬스터가 된 학생이 등장했대요! 과제에 지쳐 좀비가 된 학생이라던데...",
           "생협 앞에 있는 리무버블 스티커를 인식하면 몬스터와 전투할 수 있어요.",
           "과제로 인해 흑화한 학생을 꼭 구해주세요! 건투를 빌어요!"
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

            if (curr == 4)
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

