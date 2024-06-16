using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public string _playerName { get; set; }
    public int HP { get; set; } = 100; 
    [SerializeField] private HPBar _hpBar;
    
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    
    public int MaxHP { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
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
