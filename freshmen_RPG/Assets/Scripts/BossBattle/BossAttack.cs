using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject redProjectilePrefab;
    public GameObject blueProjectilePrefab;
    public Transform player;
    public float attackInterval = 5f;

    private Coroutine attackRoutine;

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
            FireProjectiles();
        }
    }

    void FireProjectiles()
    {

        Debug.Log("FireProjectiles called");
        // 다섯 개의 방향 계산
        for (int i = 0; i < 20; i++)
        {
            float angle = i * 360f / 20f; // 360도를 20개의 부분으로 나누어 방향 계산
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            // 빨간색 발사체 생성
            GameObject projectilePrefab = redProjectilePrefab;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<BossProjectile>().Initialize(direction, gameObject);
        }

        // 파란색 발사체 생성 (최대 하나)
        if (Random.value > 0.7f)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(blueProjectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<BossProjectile>().Initialize(direction, gameObject);
        }
    }
}
