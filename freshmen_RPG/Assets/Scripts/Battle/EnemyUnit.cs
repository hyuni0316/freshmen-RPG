using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private MonsterBase _base;
    public Monster _Monster { get; set; }

    public void SetUp()
    {
        _Monster = new Monster(_base);
        GetComponent<Image>().sprite = _Monster.MonsterSprite;
    }
}
