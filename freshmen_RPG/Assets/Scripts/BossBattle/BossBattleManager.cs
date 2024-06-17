using System.Collections;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField][Header("보스전 스토리")] GameObject[] items;
    int itemIdx = 0;
    [SerializeField] GameObject playerInfo;
    [SerializeField] GameObject skillSet;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject bossObject; // Boss 오브젝트 참조
    private BossAttack bossAttack; // BossAttack 스크립트 참조

    void Awake()
    {
        if (bossObject != null)
        {
            bossAttack = bossObject.GetComponent<BossAttack>();

            if (bossAttack == null)
            {
                Debug.LogError("BossAttack component not found on the boss object!");
            }
        }
        else
        {
            Debug.LogError("Boss object not assigned in the Inspector!");
        }
    }

    void Start()
    {
        if (items == null || items.Length == 0)
            return;

        foreach (var item in items)
        {
            item.SetActive(false);
        }

        itemIdx = -1;
        ActiveNextItem();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ActiveNextItem();
        }
    }

    public IEnumerator SwitchToPlayerCamera()
    {
        Debug.Log("SwitchToPlayerCamera called");
        mainCamera.enabled = false;
        playerCamera.enabled = true;

        yield return new WaitForSeconds(0.1f);

        // 카메라 전환 후 보스 공격 시작
        Debug.Log("Starting boss attack");
        if (bossAttack != null)
        {
            bossAttack.StartAttacking();
        }
        else
        {
            Debug.LogError("BossAttack component not found!");
        }
    }

    public void ActiveNextItem()
    {
        if (itemIdx > -1 && itemIdx < items.Length)
        {
            items[itemIdx].SetActive(false);
        }

        itemIdx++;

        if (itemIdx > -1 && itemIdx < items.Length)
        {
            items[itemIdx].SetActive(true);
        }
        else
        {
            playerInfo.SetActive(true);
            skillSet.SetActive(true);
            StartCoroutine(SwitchToPlayerCamera());
        }
    }
}