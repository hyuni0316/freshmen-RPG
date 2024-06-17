using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject redProjectilePrefab;
    public GameObject blueProjectilePrefab;
    public Transform player;
    public float attackInterval = 5f;
    public float directionChangeInterval = 2f; // 방향 변경 간격
    public GameObject modalWindow; // 모달 창

    private Coroutine attackRoutine;
    private bool canFire = true;

    void Start()
    {
        // StartCoroutine(AttackRoutine());
    }

    public void StartAttacking()
    {
        Debug.Log("StartAttacking called");
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            if (canFire)
            {
                FireProjectiles();
            }
        }
    }

    void FireProjectiles()
    {
        // 다섯 개의 방향 계산
        for (int i = 0; i < 20; i++)
        {
            Vector3 direction = Random.insideUnitSphere.normalized; // 랜덤한 방향 설정

            // 빨간색 발사체 생성
            GameObject projectilePrefab = redProjectilePrefab;

            // 발사체를 앞으로 조금 이동하여 plane 위로 발사되도록 위치 조정
            Vector3 spawnPosition = transform.position + Vector3.forward * -2f + Vector3.up * 1.5f;
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            projectile.GetComponent<BossProjectile>().Initialize(direction, gameObject);
        }

        // 파란색 발사체 생성 (최대 하나)
        if (Random.value > 0.7f)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            // 발사체를 앞으로 조금 이동하여 plane 위로 발사되도록 위치 조정
            Vector3 spawnPosition = transform.position + Vector3.forward * -2f + Vector3.up * 1.5f;
            GameObject projectile = Instantiate(blueProjectilePrefab, spawnPosition, Quaternion.identity);
            projectile.GetComponent<BossProjectile>().Initialize(direction, gameObject);
        }
    }

    public void StopFiringFor3Seconds()
    {
        if (canFire)
        {
            StartCoroutine(StopFiringCoroutine());
        }
    }

    IEnumerator StopFiringCoroutine()
    {
        canFire = false;
        if (modalWindow != null)
        {
            modalWindow.SetActive(true); // 모달 창 활성화
        }
        yield return new WaitForSeconds(3f);
        canFire = true;
        if (modalWindow != null)
        {
            modalWindow.SetActive(false); // 모달 창 비활성화
        }
    }
}
