using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerUnit : MonoBehaviour
{
    public string _playerName { get; set; }
    public int HP { get; set; }
    [SerializeField] private HPBar _hpBar;
    
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    
    public int MaxHP { get { return maxHp; } }
    public int Attack { 
        get { return attack; }
        set { attack = value; }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    private void Start()
    {
        HP = MaxHP;
        _playerName = "새로니";
    }

    public void Setup()
    {
        _hpBar.SetHP((float) HP/maxHp);
    }

    public IEnumerator UpdateHP()
    {
        yield return _hpBar.SetHPSmooth((float)HP / MaxHP);
    }

    public bool TakeDamage(int attack, int actionDamage)
    {
        float modifiers = Random.Range(0.90f, 1f);
        float d = actionDamage * ((float)attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }
        return false;
    }
}
