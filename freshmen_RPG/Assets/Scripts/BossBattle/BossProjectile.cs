using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private GameObject owner;
    public int damageAmount = 10; // 발사체의 데미지 양

    public void Initialize(Vector3 dir, GameObject ownerObject)
    {
        direction = dir.normalized;
        owner = ownerObject;
        Destroy(gameObject, 5f); // 5초 후에 발사체 삭제
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_hpbar_ui playerHP = other.GetComponent<player_hpbar_ui>();

            if (gameObject.CompareTag("Blue"))
            {
                playerHP.Heal(damageAmount);
            }
            else
            {
                // 그 외의 발사체는 플레이어에게 데미지
                playerHP.TakeDamage(damageAmount);
            }

            // 발사체 파괴
            Destroy(gameObject);
        }
    }

}
