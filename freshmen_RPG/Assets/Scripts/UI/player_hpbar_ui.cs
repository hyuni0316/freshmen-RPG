using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_hpbar_ui : MonoBehaviour
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
            return;
        }
    }

    public void Heal(int healAmount)
    {
        currHP += healAmount;
        if (currHP > maxHP)
            currHP = maxHP;
        UpdateHPBar();
    }
}
