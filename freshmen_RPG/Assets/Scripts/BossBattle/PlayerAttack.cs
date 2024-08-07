using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform boss;
    public Button attackButton;
    public int damageAmount = 20; // 공격 데미지 양

    void Start()
    {
        attackButton.onClick.AddListener(FireProjectile);
    }

    void FireProjectile()
    {
        Debug.Log("플레이어 공격 called");

        // 플레이어의 현재 위치
        Vector3 playerPosition = transform.position;

        // 플레이어에서 보스를 향하는 방향 벡터 계산
        Vector3 directionToBoss = (boss.position - playerPosition).normalized;

        // 발사체 생성 및 방향 설정
        Vector3 offset = new Vector3(0, 1.5f, 0);
        GameObject projectile = Instantiate(projectilePrefab, playerPosition + offset, Quaternion.identity);

        // 발사체 초기화
        projectile.GetComponent<PlayerProjectile>().Initialize(directionToBoss, gameObject);
        Debug.Log("공격함");
    }

}
