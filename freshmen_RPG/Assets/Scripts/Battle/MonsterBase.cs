using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Create new monster.")]
public class MonsterBase : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private Sprite _monsterSprite;

    [SerializeField] private int maxHP;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    
    // properties
    public string Name { get { return name; } }
    public Sprite MonsterSprite { get { return _monsterSprite; } }
    public int MaxHP { get { return maxHP; } }
    public int Attack { get { return attack; }
        set { attack = value; }
    }
    public int Defense { get { return defense; }
        set { defense = value; }
    }

}
