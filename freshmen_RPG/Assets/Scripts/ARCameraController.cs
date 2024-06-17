using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCameraController : MonoBehaviour
{
    [SerializeField] private CurrentSituation _currentSituation;
    [SerializeField] private BattleDialogBox _dialogBox;

    public void LoadSceneHakmoonInfo()
    {
        StartCoroutine(SceneLoader("", true, "학생문화관"));
    }

    public void LoadSceneHakmoonBattle()
    {
        StartCoroutine(SceneLoader("HakmoonBattleScene", _currentSituation.HakmoonInfo, "학생문화관"));
    }
    
    public void LoadScenePoscoInfo()
    { 
        StartCoroutine(SceneLoader("", _currentSituation.HakmoonBattle, "포스코관"));
    }
    
    public void LoadScenePoscoBattle()
    {
        StartCoroutine(SceneLoader("PoscoBattleScene", _currentSituation.PoscoInfo, "포스코관"));
    }
    
    public void LoadSceneAsanInfo()
    {
        StartCoroutine(SceneLoader("", _currentSituation.PoscoBattle, "공학관"));
    }
    
    public void LoadSceneAsanBattle()
    {
        StartCoroutine(SceneLoader("AsanBattleScene", _currentSituation.AsanInfo, "공학관"));
    }

    IEnumerator SceneLoader(string sceneName, bool isLoadable, string sceneNameForPrint)
    {
        if (!isLoadable)
        {
            yield return _dialogBox.TypeDialog($"아직 진행할 수 없는 단계입니다.");
            yield break;
        }
        yield return _dialogBox.TypeDialog($"{sceneNameForPrint}으로 이동합니다.");
        SceneManager.LoadScene(sceneName);
    }
    
    
}
