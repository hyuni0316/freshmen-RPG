using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private CurrentSituation _currentSituation;
    [SerializeField] private List<GameObject> _itemsGoList;
    [SerializeField] private List<Button> _infoBtnList;
    [SerializeField] private List<Button> _storyBtnList;
    
    void Start()
    {
        InitCurSituation();
    }
    
    public void InitCurSituation()
    {
        if (_currentSituation.HakmoonBattle)
        {
            _infoBtnList[0].interactable = true;
            _infoBtnList[0].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/enemyIcon_green");
            _infoBtnList[0].gameObject.GetComponent<stage_btn>()._isStageCleared = true;
            
            _storyBtnList[1].interactable = true;
            _storyBtnList[1].gameObject.GetComponent<Image>().color = new Color(168f, 209f, 170f, 255f);
            _itemsGoList[0].SetActive(true);
        }

        if (_currentSituation.PoscoBattle)
        {
            _infoBtnList[1].interactable = true;
            _infoBtnList[1].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/enemyIcon_green");
            _infoBtnList[1].gameObject.GetComponent<stage_btn>()._isStageCleared = true;
            
            _storyBtnList[2].interactable = true;
            _storyBtnList[2].gameObject.GetComponent<Image>().color = new Color(168f, 209f, 170f,255f);
            _itemsGoList[1].SetActive(true);
        }

        if (_currentSituation.AsanBattle)
        {
            _infoBtnList[2].interactable = true;
            _infoBtnList[2].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/enemyIcon_green");
            _infoBtnList[2].gameObject.GetComponent<stage_btn>()._isStageCleared = true;
            
            _storyBtnList[3].interactable = true;
            _storyBtnList[3].gameObject.GetComponent<Image>().color = new Color(168f, 209f, 170f, 255f);
            _itemsGoList[2].SetActive(true);
        }

        if (_currentSituation.BossBattle)
        {
            _storyBtnList[4].interactable = true;
            _storyBtnList[4].gameObject.GetComponent<Image>().color = new Color(168f, 209f, 170f);
        }
    }
}
