using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private HPBar _hpBar;
    [SerializeField] private TextMeshProUGUI _hpTxt;

    private Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;
        _nameText.text = monster.Base.Name;
        _hpBar.SetHP( (float) monster.HP / monster.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        _hpTxt.text = $"{_monster.HP}/{_monster.MaxHP}";
        yield return _hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHP);
    }
}
