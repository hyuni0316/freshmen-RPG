using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MonsterState {None, FirstPhase, SecondPhase, ThirdPhase}
public class Monster
{
    private MonsterBase _monsterBase;

    private int hp;
    private int attack;
    private int defense;

    public MonsterState MonsterState { get; set; }
    public string _monsterName { get; set; }

    public Monster(MonsterBase mBase)
    {
        _monsterBase = mBase;
        _monsterName = mBase.Name;
        MonsterState = MonsterState.None;
        hp = mBase.MaxHP;
        attack = mBase.Attack;
        defense = mBase.Defense;
    }
    
    // properties
    public MonsterBase Base { get { return _monsterBase; } }
    public int MaxHP { get { return _monsterBase.MaxHP; } }
    
    public int HP
    {
        get { return hp;}
        set { hp = value; }
    }
    public int Attack {
        get { return attack; }
        set
        {
            if (value <= 5)
            {
                attack = 5;
                return;
            } 
            attack = value; }
    }
    public int Defense { 
        get { return defense; }
        set {
            if (value <= 5)
            {
                defense = 5;
                return;
            } 
            defense = value;
        }
    }
    public Sprite MonsterSprite { get { return _monsterBase.MonsterSprite; } }

    // return 값은 진행해야하는 OX퀴즈 번호
    public int TakeDamage(int attack, int actionDamage)
    { 
        float modifiers = Random.Range(0.90f, 1f);
        float d = actionDamage * ((float)attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0 && MonsterState == MonsterState.SecondPhase)
        {
            HP = 0;
            MonsterState = MonsterState.ThirdPhase;
            return 2;
        }
        else if (HP <= MaxHP * 0.33f && MonsterState == MonsterState.FirstPhase)
        {
            HP = Mathf.FloorToInt(MaxHP * 0.33f);
            MonsterState = MonsterState.SecondPhase;
            return 1;
        }
        else if (HP <= MaxHP * 0.66f  && MonsterState == MonsterState.None)
        {
            HP = Mathf.FloorToInt(MaxHP * 0.66f);
            MonsterState = MonsterState.FirstPhase;
            return 0;
        }
        return -1;
    }
}
