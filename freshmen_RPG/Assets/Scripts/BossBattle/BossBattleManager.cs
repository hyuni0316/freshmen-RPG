using System.Collections;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField][Header("보스전 스토리")] GameObject[] items;
    int itemIdx = 0;
    [SerializeField] GameObject playerInfo;
    [SerializeField] GameObject skillSet;
    public Camera mainCamera;
    public Camera playerCamera;

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
        mainCamera.enabled = false;
        playerCamera.enabled = true;

        yield return new WaitForSeconds(0.1f);
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
