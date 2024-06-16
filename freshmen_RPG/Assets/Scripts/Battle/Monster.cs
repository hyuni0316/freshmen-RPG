using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState {None, FirstPhase, SecondPhase, ThirdPhase}
public class Monster
{
    private MonsterBase _monsterBase;

    public MonsterState MonsterState { get; set; }
    public string _monsterName { get; set; }
    public int HP { get; set; }

    public Monster(MonsterBase mBase)
    {
        _monsterBase = mBase;
        _monsterName = mBase.Name;
        HP = mBase.MaxHP;
        MonsterState = MonsterState.None;
    }
    
    // properties
    public MonsterBase Base { get { return _monsterBase; } }
    public int MaxHP { get { return _monsterBase.MaxHP; } }
    public int Attack { get { return _monsterBase.Attack; }
        set { _monsterBase.Attack = value; }
    }
    public int Defense { get { return _monsterBase.Defense; } }
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
        else if (HP <= MaxHP * 0.33f)
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
