using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private GameObject owner;
    public int damageAmount = 20; // 공격 데미지 양

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
        if (other.CompareTag("Boss"))
        {
            boss_hpbar_ui bossHP = other.GetComponent<boss_hpbar_ui>();
            bossHP.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}

