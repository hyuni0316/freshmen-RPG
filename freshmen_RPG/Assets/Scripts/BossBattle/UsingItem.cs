using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UsingItem : MonoBehaviour
{
    public Button targetButton1;
    public Button targetButton2;
    public Button targetButton3;
    public Transform target; // 버튼이 날아갈 타겟
    public GameObject itemSetModal; // 비활성화할 모달창
    public GameObject infoModal1; // 버튼1 클릭 시 표시될 안내 모달 이미지
    public GameObject infoModal2; // 버튼2 클릭 시 표시될 안내 모달 이미지
    public GameObject infoModal3; // 버튼3 클릭 시 표시될 안내 모달 이미지
    public float animationDuration = 1f; // 애니메이션 지속 시간
    public float flyDuration = 1.5f; // 날아가는 애니메이션 지속 시간
    public float spinSpeed = 360f; // 회전 속도
    public float delayBeforeModalDisable = 0.5f; // 모달창 비활성화 전 대기 시간
    public float infoModalDuration = 5f; // 안내 모달창 표시 시간

    void Start()
    {
        AddButtonListeners(targetButton1, infoModal1);
        AddButtonListeners(targetButton2, infoModal2);
        AddButtonListeners(targetButton3, infoModal3);
    }

    void AddButtonListeners(Button button, GameObject infoModal)
    {
        if (button != null)
        {
            ButtonFlyAnimator animator = button.gameObject.AddComponent<ButtonFlyAnimator>();
            animator.Initialize(button, target, animationDuration, flyDuration, spinSpeed, itemSetModal, delayBeforeModalDisable, infoModal, infoModalDuration);
        }
    }
}

public class ButtonFlyAnimator : MonoBehaviour
{
    private Button targetButton;
    private Transform target;
    private float animationDuration;
    private float flyDuration;
    private float spinSpeed;
    private GameObject itemSetModal;
    private float delayBeforeModalDisable;
    private GameObject infoModal;
    private float infoModalDuration;

    private RectTransform buttonRectTransform;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Image buttonImage;
    private Color originalColor;

    public void Initialize(Button button, Transform targetTransform, float animDuration, float flyDur, float spinSpd, GameObject modal, float delay, GameObject info, float infoDur)
    {
        targetButton = button;
        target = targetTransform;
        animationDuration = animDuration;
        flyDuration = flyDur;
        spinSpeed = spinSpd;
        itemSetModal = modal;
        delayBeforeModalDisable = delay;
        infoModal = info;
        infoModalDuration = infoDur;

        buttonRectTransform = targetButton.GetComponent<RectTransform>();
        buttonImage = targetButton.GetComponent<Image>();
        originalScale = buttonRectTransform.localScale;
        originalPosition = buttonRectTransform.position;
        originalRotation = buttonRectTransform.rotation;
        originalColor = buttonImage.color;

        targetButton.onClick.AddListener(() => StartCoroutine(AnimateButton()));
    }

    IEnumerator AnimateButton()
    {
        // Canvas의 sortingOrder 설정
        Canvas canvas = targetButton.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            // Canvas의 sortingOrder를 최상위로 설정
            canvas.overrideSorting = true;
            canvas.sortingOrder = 999; // 원하는 숫자로 설정 (최상위로 올릴 숫자)
        }

        // 자신의 Sibling Index를 가장 큰 값으로 설정하여 최상위에 그리기
        targetButton.transform.SetAsLastSibling();

        // 모달창 표시
        if (infoModal != null)
        {
            infoModal.SetActive(true);

            // 자식 GameObject들을 검색해서 Image 및 Text 컴포넌트를 찾아서 투명도를 조절
            foreach (Transform child in infoModal.transform)
            {
                Image imageComponent = child.GetComponent<Image>();
                if (imageComponent != null)
                {
                    StartCoroutine(FadeImage(imageComponent, infoModalDuration));
                }

                Text textComponent = child.GetComponent<Text>();
                if (textComponent != null)
                {
                    StartCoroutine(FadeText(textComponent, infoModalDuration));
                }
            }
        }

        // 아이템 이미지 점점 확대
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * 10f; // 화면을 가득 채우기 위해 큰 값으로 설정
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        while (elapsedTime < animationDuration)
        {
            // 모달창 활성화와 동시에 버튼 이미지 커지는 애니메이션 진행
            if (infoModal != null)
            {
                buttonRectTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / animationDuration);
                buttonRectTransform.position = Vector3.Lerp(originalPosition, screenCenter, elapsedTime / animationDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (infoModal != null)
        {
            buttonRectTransform.localScale = targetScale;
            buttonRectTransform.position = screenCenter;
        }

        // 타겟을 향해 날아가는 애니메이션
        elapsedTime = 0f;
        Vector3 startPosition = buttonRectTransform.position;
        Vector3 endPosition = Camera.main.WorldToScreenPoint(target.position); // 타겟의 화면 좌표

        while (elapsedTime < flyDuration)
        {
            buttonRectTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / flyDuration);
            buttonRectTransform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);

            // 서서히 사라짐
            if (infoModal != null)
            {
                foreach (Transform child in infoModal.transform)
                {
                    Image imageComponent = child.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        Color imageColor = imageComponent.color;
                        imageColor.a = Mathf.Lerp(1f, 0f, elapsedTime / flyDuration); // 투명도 서서히 감소
                        imageComponent.color = imageColor;
                    }

                    Text textComponent = child.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        Color textColor = textComponent.color;
                        textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / flyDuration); // 투명도 서서히 감소
                        textComponent.color = textColor;
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 버튼을 원래 위치와 크기로 되돌리고 회전도 초기화 및 비활성화, 색상 회색으로 변경
        buttonRectTransform.localScale = originalScale;
        buttonRectTransform.position = originalPosition;
        buttonRectTransform.rotation = originalRotation;

        // 모든 자식 객체의 색상을 투명하게 만듦
        if (infoModal != null)
        {
            foreach (Transform child in infoModal.transform)
            {
                Image imageComponent = child.GetComponent<Image>();
                if (imageComponent != null)
                {
                    Color imageColor = imageComponent.color;
                    imageColor.a = 0f;
                    imageComponent.color = imageColor;
                }

                Text textComponent = child.GetComponent<Text>();
                if (textComponent != null)
                {
                    Color textColor = textComponent.color;
                    textColor.a = 0f;
                    textComponent.color = textColor;
                }
            }

            infoModal.SetActive(false);
        }

        // Canvas sortingOrder 초기화
        if (canvas != null)
        {
            canvas.overrideSorting = false;
        }

        targetButton.interactable = false;

        // 딜레이 후 모달창 비활성화
        yield return new WaitForSeconds(delayBeforeModalDisable);
        if (itemSetModal != null)
        {
            itemSetModal.SetActive(false);
        }
    }


    IEnumerator FadeImage(Image image, float duration)
    {
        float elapsedTime = 0f;
        Color initialColor = image.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / duration);
            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }

    IEnumerator FadeText(Text text, float duration)
    {
        float elapsedTime = 0f;
        Color initialColor = text.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / duration);
            text.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }


    IEnumerator HideInfoModalAfterDelay(GameObject modal, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Transform child in modal.transform)
        {
            Image imageComponent = child.GetComponent<Image>();
            if (imageComponent != null)
            {
                Color imageColor = imageComponent.color;
                imageColor.a = 0f; // 투명도 설정
                imageComponent.color = imageColor;
            }

            Text textComponent = child.GetComponent<Text>();
            if (textComponent != null)
            {
                Color textColor = textComponent.color;
                textColor.a = 0f; // 투명도 설정
                textComponent.color = textColor;
            }
        }

        modal.SetActive(false);
    }

}
