using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class stage_btn : MonoBehaviour
{
    public bool _isStageCleared = false;
    public StageType _stageType = StageType.None;
    [SerializeField] private GameObject _popup_ui;

    public void OnClick()
    {
        if (_isStageCleared)
        {
            _popup_ui.SetActive(true);
        }
    }
}
