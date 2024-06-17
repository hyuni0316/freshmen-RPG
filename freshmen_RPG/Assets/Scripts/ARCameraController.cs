using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCameraController : MonoBehaviour
{
    [SerializeField] private CurrentSituation _currentSituation;

    public void LoadSceneHakmoonInfo()
    {
        SceneManager.LoadScene("");
    }

    public void LoadSceneHakmoonBattle()
    {
        if (_currentSituation.HakmoonInfo)
        {
            SceneManager.LoadScene("HakmoonBattleScene");
        }
    }
    
    public void LoadScenePoscoInfo()
    {
        if (_currentSituation.HakmoonBattle)
        {
            SceneManager.LoadScene("");
        }
    }
    
    public void LoadScenePoscoBattle()
    {
        if (_currentSituation.PoscoInfo)
        {
            SceneManager.LoadScene("PoscoBattleScene");
        }
    }
    
    public void LoadSceneAsanInfo()
    {
        if (_currentSituation.PoscoBattle)
        {
            SceneManager.LoadScene("");
        }
    }
    
    public void LoadSceneAsanBattle()
    {
        if (_currentSituation.AsanInfo)
        {
            SceneManager.LoadScene("AsanBattleScene");
        }
    }
    
    
}
