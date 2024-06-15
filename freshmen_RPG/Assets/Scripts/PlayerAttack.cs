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
        // 플레이어에서 보스를 향하는 방향 벡터 계산
        Vector3 directionToBoss = (boss.position - transform.position).normalized;

        // 발사체 생성 및 방향 설정
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<PlayerProjectile>().Initialize(directionToBoss, gameObject);
    }
}
