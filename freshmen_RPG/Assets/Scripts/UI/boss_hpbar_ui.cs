using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss_hpbar_ui : MonoBehaviour
{
    public int maxHP;
    private int currHP;
    public Slider HPBar;

    void Start()
    {
        currHP = maxHP;
        UpdateHPBar();
    }

    public void SetHP(int hp)
    {
        maxHP = hp;
        currHP = maxHP;
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        if (HPBar != null)
        {
            HPBar.value = (float)currHP / maxHP;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currHP <= 0)
            return;
        currHP -= damage;
        UpdateHPBar();
        if (currHP <= 0)
        {
            // 사망 코드
            Debug.Log(gameObject.name + " is dead");
            Destroy(gameObject); //이거 3d 리소스 찾으면 바꾸기
            return;
        }
    }
}
